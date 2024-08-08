using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProPresenter.Api.Configuration;

namespace ProPresenter.Api.Extensions;

public static class ProPresenterExtensions
{
    public static void AddProPresenterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("Config");
        var baseUrl = config.GetValue<string>("BaseUrl");
        ArgumentException.ThrowIfNullOrEmpty(baseUrl);
        services.AddOptions();
        services.Configure<Config>(x => config.Bind(x));
        services.AddHttpClient("ProPresenter", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
        services.AddScoped<IProPresenterClient, ProPresenterClient>();
    }
    
}