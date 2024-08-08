namespace ProPresenter.Api;

public interface IProPresenterClient
{
    Task<Messages.Messages?> GetMessageAsync(string id);
    Task TriggerMessageAsync(string name, string room);
}