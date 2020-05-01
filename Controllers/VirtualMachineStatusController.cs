using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using Microsoft.AspNetCore.Cors;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineStatusController : ControllerBase
    {
        private readonly VirtualMachineContext _context;

        public VirtualMachineStatusController(VirtualMachineContext context)
        {
            _context = context;
        }

        // GET: api/VirtualMachineStatus
        [HttpGet]
        [EnableCors(Startup.CorsPolicyName)]
        public async Task<ActionResult<IEnumerable<VirtualMachine>>> GetVirtualMachines()
        {
            return await _context.VirtualMachines.ToListAsync();
        }

        // GET: api/VirtualMachineStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VirtualMachine>> GetVirtualMachine(int id)
        {
            var virtualMachine = await _context.VirtualMachines.FindAsync(id);

            if (virtualMachine == null)
            {
                return NotFound();
            }

            return virtualMachine;
        }

        [HttpGet("New")]
        [EnableCors(Startup.CorsPolicyName)]
        public async Task<ActionResult<IEnumerable<VirtualMachine>>> GetNewVirtualMachines()
        {
            return new ActionResult<IEnumerable<VirtualMachine>>(await _context.GetNewVirtualMachines());
        }

        // PUT: api/VirtualMachineStatus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVirtualMachine(int id, VirtualMachine virtualMachine)
        {
            if (id != virtualMachine.Id)
            {
                return BadRequest();
            }

            _context.Entry(virtualMachine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VirtualMachineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VirtualMachineStatus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<VirtualMachine>> PostVirtualMachine(VirtualMachine virtualMachine)
        {
            _context.VirtualMachines.Add(virtualMachine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVirtualMachine", new { id = virtualMachine.Id }, virtualMachine);
        }

        // DELETE: api/VirtualMachineStatus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VirtualMachine>> DeleteVirtualMachine(int id)
        {
            var virtualMachine = await _context.VirtualMachines.FindAsync(id);
            if (virtualMachine == null)
            {
                return NotFound();
            }

            _context.VirtualMachines.Remove(virtualMachine);
            await _context.SaveChangesAsync();

            return virtualMachine;
        }

        private bool VirtualMachineExists(int id)
        {
            return _context.VirtualMachines.Any(e => e.Id == id);
        }
    }
}
