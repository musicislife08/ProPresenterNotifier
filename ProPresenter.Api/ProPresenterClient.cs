using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProPresenter.Api.Configuration;
using ProPresenter.Api.Messages;

namespace ProPresenter.Api;

public class ProPresenterClient: IProPresenterClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProPresenterClient> _logger;
    private readonly Config _config;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public ProPresenterClient(IHttpClientFactory httpClientFactory, ILogger<ProPresenterClient> logger, IOptions<Config> config)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("ProPresenter");
        _config = config.Value;
        ArgumentException.ThrowIfNullOrEmpty(_config.MessageName);
        _logger.LogDebug("{Name} Initialized", nameof(ProPresenterClient));
    }

    private async Task<string?> GetMessageId()
    {
        _logger.LogDebug("{Name} Called", nameof(GetMessageId));
        var request = new HttpRequestMessage(HttpMethod.Get, MessageEndpoints.Messages);
        try
        {
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<List<Messages.Messages>>(content, _jsonOptions);
            var id = responseObject?.Single(x => x.Id?.Name == _config.MessageName);
            return id?.Id?.Uuid;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
            return null;
        }
    }

    public async Task<Messages.Messages?> GetMessageAsync(string id)
    {
        _logger.LogDebug("{Name} Called", nameof(GetMessageAsync));
        var request = new HttpRequestMessage(HttpMethod.Get, string.Format(MessageEndpoints.Messages, id));
        try
        {
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Messages.Messages>(content, _jsonOptions);
            return responseObject;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
            return null;
        }
    }

    public async Task TriggerMessageAsync(string name, string room)
    {
        var contentObject = new List<Token>()
        {
            new()
            {
                Name = "Name",
                Text = new TextToken
                {
                    TextContent = name,
                }
            },
            new()
            {
                Name = "Room",
                Text = new TextToken
                {
                    TextContent = room,
                }
            }
        };
        var id = await GetMessageId();
        if (string.IsNullOrWhiteSpace(id))
            throw new NullReferenceException("MessageId is null");
        var request = new HttpRequestMessage(HttpMethod.Post, string.Format(MessageEndpoints.MessageTrigger, id));
        var rawContent = JsonSerializer.Serialize(contentObject, _jsonOptions);
        request.Content = new StringContent(rawContent, Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return;
            _logger.LogError("Unable to send message: {Reason}", response.ReasonPhrase);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
        }
    }
    
}