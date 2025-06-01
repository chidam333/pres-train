public enum AccountKind
{
    Savings,
    Current,
    Salaried
}
public class Account
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public AccountKind AccountType { get; set; } = AccountKind.Savings;
}