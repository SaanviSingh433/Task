using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using WebApplication4.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public AccountController(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
            if(_dbcontext.Accounts == null)
            {
                return NotFound();
            }
            return await _dbcontext.Accounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            if (_dbcontext.Accounts == null)
            {
                return NotFound();
            }
            var account = await _dbcontext.Accounts.FindAsync(id);
            if(account == null)
            {
                return NotFound();
            }
            return account;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _dbcontext.Accounts.Add(account);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpPut]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if(id != account.Id)
            {
                return BadRequest();

            }
            _dbcontext.Entry(account).State = EntityState.Modified;
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            
            catch(DbUpdateConcurrencyException)
            {
                if(!AccountAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();

        }

        private bool AccountAvailable(int id)
        {
            return (_dbcontext.Accounts?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if(_dbcontext.Accounts == null)
            {
                return NotFound();
            }

            var account = await _dbcontext.Accounts.FindAsync(id);
            if( account == null)
            {
                return NotFound();
            }

            _dbcontext.Accounts.Remove(account);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }

        
        /*private readonly List<Account> accounts = new List<Account>();

         [HttpGet]
         public IEnumerable<Account> Get()
         {
             return accounts;
         }

         [HttpPost]
         public IActionResult Post([FromBody] Account account)
         {
             accounts.Add(account);
             return Ok(account);


         }

         [HttpPut("{id}")]
         public IActionResult Put(int id, [FromBody] Account account)
         {
             var existingAccount = accounts.FirstOrDefault(a => a.Id == id);
             if (existingAccount == null)
             {
                 return NotFound();
             }

             existingAccount.Name = account.Name;
             existingAccount.ModifiedDate = account.ModifiedDate;
             return Ok(existingAccount);
         }

         [HttpDelete("{id}")]
         public IActionResult Delete(int id)
         {
             var accountToRemove = accounts.FirstOrDefault(a => a.Id == id);
             if (accountToRemove == null)
             {
                 return NotFound();
             }
             accounts.Remove(accountToRemove);
             return Ok();
         }*/
    }
}
