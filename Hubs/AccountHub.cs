using Microsoft.AspNetCore.SignalR;
using SignalR.Models;

namespace SignalR.Hubs;

public class AccountHub : Hub
{
    private readonly Account _account;

    public AccountHub(Account account)
    {
        _account = account;
    }

    public Task ConnectToHub()
    {
        Groups.AddToGroupAsync(Context.ConnectionId, nameof(AccountHub));

        Clients.All.SendAsync("ChangedState", new AccountViewModel(_account));

        return Task.CompletedTask;
    }
}
