namespace Cnab.Domain.Model;

public class Store : Entity
{
    public string Name { get; init; } = string.Empty;
    public string Owner { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    private ICollection<StoreTransaction> transactions { get; set; } = new List<StoreTransaction>();

    public Store(string name, string owner, decimal balance) : base()
    {
        Name = name;
        Owner = owner;
        Balance = balance;
    }

    public Store()
    {

    }
}
