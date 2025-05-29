public enum TransactionType
{
    Credit,
    Debit
}
public class Transaction
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public required TransactionType TransactionType { get; set; }
    public required int AccountId { get; set; }
    public Account? Account { get; set; }
}