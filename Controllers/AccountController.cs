using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Hubs;
using SignalR.Models;

namespace SignalR.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly Account _account;
    private readonly IHubContext<AccountHub> _hubContext;

    public AccountController(Account account, IHubContext<AccountHub> hubContext)
    {
        _account = account;
        _hubContext = hubContext;
    }

    [HttpPost("Deposit/{amount:double}")]
    public async Task<IActionResult> Deposit(double amount)
    {
        _account.Deposit(amount);
        await SendChangedStateToHubClients(_account);
        return Ok();
    }

    [HttpPost("Withdraw/{amount:double}")]
    public async Task<IActionResult> Withdraw(double amount)
    {
        _account.Withdraw(amount);
        await SendChangedStateToHubClients(_account);
        return Ok();
    }

    [HttpPost("PayInterest")]
    public async Task<IActionResult> PayInterest()
    {
        _account.PayInterest();
        await SendChangedStateToHubClients(_account);
        return Ok();
    }

    private async Task SendChangedStateToHubClients(Account account)
    {
        await _hubContext.Clients.All.SendAsync("ChangedState", new AccountViewModel(account));
    }
}
