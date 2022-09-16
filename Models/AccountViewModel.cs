namespace SignalR.Models;

public class AccountViewModel
{
    public string Balance { get; set; }
    public string State { get; set; }
    public string Owner { get; set; }

    public AccountViewModel() { }
    public AccountViewModel(Account account)
    {
        Owner = account.Owner;
        Balance = Balance = account.Balance.ToString("c");
        State = GetState(account.State.GetType());
    }

    private string GetState(Type type)
    {
        if (type.IsAssignableFrom(typeof(GoldState)))
            return "Gold";

        return type.IsAssignableFrom(typeof(SilverState)) ? "Silver" : "Red";
    }
}
