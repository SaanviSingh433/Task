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
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public AppointmentController(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointment()
        {
            if (_dbcontext.Appointments == null)
            {
                return NotFound();
            }
            return await _dbcontext.Appointments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            if (_dbcontext.Appointments == null)
            {
                return NotFound();
            }
            var appointment = await _dbcontext.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            _dbcontext.Appointments.Add(appointment);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        [HttpPut]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();

            }
            _dbcontext.Entry(appointment).State = EntityState.Modified;
            try
            {
                await _dbcontext.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentAvailable(id))
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

        private bool AppointmentAvailable(int id)
        {
            return (_dbcontext.Appointments?.Any(x => x.Id == id)).GetValueOrDefault();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            if (_dbcontext.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _dbcontext.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _dbcontext.Appointments.Remove(appointment);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }
        /*private readonly List<Appointment> appointments = new List<Appointment>();

        [HttpGet]
        public IEnumerable<Appointment> Get()
        {
            return  appointments ;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Appointment appointment)
        {
           appointments.Add(appointment);
            return Ok(appointment);


        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Appointment appointment)
        {
            var existingAppointment = appointments.FirstOrDefault(a => a.Id == id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            existingAppointment.ClientId = appointment.ClientId;
            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.EndTime = appointment.EndTime;
            existingAppointment.ModifiedDate = appointment.ModifiedDate;
            return Ok(existingAppointment);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var appointmentToRemove = appointments.FirstOrDefault(a => a.Id == id);
            if (appointmentToRemove == null)
            {
                return NotFound();
            }
            appointments.Remove(appointmentToRemove);
            return Ok();
        }*/
    }
}

