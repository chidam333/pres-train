using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bankingApp.Contexts;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly BankingContext _context;

    public AccountController(BankingContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
    {
        var accounts = await _context.Accounts.ToListAsync();
        return Ok(accounts);
    }

    [HttpPost]
    public async Task<ActionResult<Account>> PostAccount([FromBody] Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return Created("", account);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Account>> PutAccount(int id, [FromBody] Account account)
    {
        var existingAccount = await _context.Accounts.FindAsync(id);
        if (existingAccount == null)
        {
            return NotFound();
        }
        existingAccount.Name = account.Name;
        existingAccount.Balance = account.Balance;
        existingAccount.AccountType = account.AccountType;
        
        await _context.SaveChangesAsync();
        return Ok(existingAccount);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}