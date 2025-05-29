using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bankingApp.Contexts;

[ApiController]
[Route("/api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly BankingContext _context;

    public TransactionController(BankingContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        var transactions = await _context.Transactions
            .Include(t => t.Account)
            .ToListAsync();
        return Ok(transactions);
    }

    [HttpGet("{accountId:int}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByAccount(int accountId)
    {
        var transactions = await _context.Transactions
            .Include(t => t.Account)
            .Where(t => t.AccountId == accountId)
            .ToListAsync();
        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> PostTransaction([FromBody] Transaction transaction)
    {
        var account = await _context.Accounts.FindAsync(transaction.AccountId);
        if (account == null)
        {
            return BadRequest("Account not found");
        }

        if (transaction.TransactionType == TransactionType.Credit)
        {
            account.Balance += transaction.Amount;
        }
        else if (transaction.TransactionType == TransactionType.Debit)
        {
            if (account.Balance < transaction.Amount)
            {
                return BadRequest("Insufficient balance");
            }
            account.Balance -= transaction.Amount;
        }

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        await _context.Entry(transaction)
            .Reference(t => t.Account)
            .LoadAsync();
            
        return Created("", transaction);
    }
}
