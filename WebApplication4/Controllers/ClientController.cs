using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public ClientController(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient()
        {
            if (_dbcontext.Clients == null)
            {
                return NotFound();
            }
            return await _dbcontext.Clients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            if (_dbcontext.Clients == null)
            {
                return NotFound();
            }
            var client = await _dbcontext.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return client;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _dbcontext.Clients.Add(client);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        [HttpPut]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();

            }
            _dbcontext.Entry(client).State = EntityState.Modified;
            try
            {
                await _dbcontext.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ClientAvailable(id))
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

        private bool ClientAvailable(int id)
        {
            return (_dbcontext.Clients?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_dbcontext.Clients == null)
            {
                return NotFound();
            }

            var client = await _dbcontext.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _dbcontext.Clients.Remove(client);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }
        /*private readonly List<Client> clients = new List<Client>();

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return clients;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            clients.Add(client);
            return Ok(client);


        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            var existingClient = clients.FirstOrDefault(c => c.Id == id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = client.Name;
            existingClient.AccountId = client.AccountId;
            existingClient.ModifiedDate = client.ModifiedDate;
            return Ok(existingClient);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var clientToRemove = clients.FirstOrDefault(c => c.Id == id);
            if (clientToRemove == null)
            {
                return NotFound();
            }
            clients.Remove(clientToRemove);
            return Ok();
        }*/
    }
}

