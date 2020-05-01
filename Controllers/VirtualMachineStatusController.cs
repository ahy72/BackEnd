using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VirtualMachineStatusController : ControllerBase
    {
        private readonly VirtualMachineContext _context;

        public VirtualMachineStatusController(VirtualMachineContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EnableCors(Startup.CorsPolicyName)]
        public async Task<VirtualMachine[]> Get()
        {
            return await _context.VirtualMachines.ToArrayAsync();
        }
    }
}
