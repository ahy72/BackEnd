using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VirtualMachineStatusController : ControllerBase
    {
        private readonly ILogger<VirtualMachineStatusController> _logger;

        public VirtualMachineStatusController(ILogger<VirtualMachineStatusController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [EnableCors(Startup.CorsPolicyName)]
        public IEnumerable<VirtualMachine> Get()
        {
            yield return new VirtualMachine(1, "ERP", 20000)
            {
                ConnectedMachine = "172.16.0.0",
                Operation = OperationStatus.Work
            };

            yield return new VirtualMachine(2, "8.1", 20001)
            {
                ConnectedMachine = "172.16.0.1",
                Operation = OperationStatus.Stop
            };
        }
    }
}
