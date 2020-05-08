using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics;

namespace BackEnd.Controllers
{
	[Route("[controller]")]
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

		[HttpPost("Refresh")]
		[EnableCors(Startup.CorsPolicyName)]
		public async Task<ActionResult<IEnumerable<VirtualMachine>>> RefreshVirtualMachines()
		{
			foreach (var machine in await _context.GetNewVirtualMachines())
			{
				_context.Entry(machine).State = EntityState.Modified;
			}

			var refreshTime = await GetRefreshTimeEntity();
			refreshTime.Time = DateTime.Now;
			_context.Entry(refreshTime).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return new ActionResult<IEnumerable<VirtualMachine>>(await _context.VirtualMachines.ToListAsync());
		}

		[HttpGet("RefreshTime")]
		[EnableCors(Startup.CorsPolicyName)]
		public async Task<ActionResult<DateTime>> GetRefreshTime()
		{
			return new ActionResult<DateTime>((await GetRefreshTimeEntity()).Time);
		}

		private async Task<RefreshTime> GetRefreshTimeEntity()
		{
			var refreshTime = await _context.RefreshTime.FirstOrDefaultAsync();
			if (refreshTime == null)
			{
				await _context.RefreshTime.AddAsync(new RefreshTime() { Time = DateTime.MinValue });
				await _context.SaveChangesAsync();
				refreshTime = await _context.RefreshTime.FirstAsync();
			}

			return refreshTime;
		}

		[HttpGet("Reset")]
		[EnableCors(Startup.CorsPolicyName)]
		public async Task<ActionResult<IEnumerable<VirtualMachine>>> ResetVirtualMachines()
		{
			foreach (var machine in _context.VirtualMachines)
			{
				machine.Operation = OperationStatus.Work;
				machine.ConnectedMachine = $"con-{machine.Name}";
				_context.Entry(machine).State = EntityState.Modified;
			}

			await _context.SaveChangesAsync();

			return new ActionResult<IEnumerable<VirtualMachine>>(await _context.VirtualMachines.ToListAsync());
		}

		[HttpGet("Message")]
		[EnableCors(Startup.CorsPolicyName)]
		public async Task<ActionResult<string>> GetMessage()
		{
			return new ActionResult<string>((await _context.Message.FirstOrDefaultAsync())?.Text ?? string.Empty);
		}

		[HttpPost("Message")]
		[EnableCors(Startup.CorsPolicyName)]
		public async Task<ActionResult<string>> PostMessageFromBody([FromBody] string message)
		{
			return await PostMessage(message);
		}

		[HttpPost("Message/{message}")]
		[EnableCors(Startup.CorsPolicyName)]
		public async Task<ActionResult<string>> PostMessage(string message)
		{
			var entity = await _context.Message.FirstOrDefaultAsync();
			if (entity == null)
			{
				await _context.Message.AddAsync(new Message() { Text = message });
			}
			else
			{
				entity.Text = message;
				_context.Entry(entity).State = EntityState.Modified;
			}

			await _context.SaveChangesAsync();
			entity = await _context.Message.FirstAsync();

			return new ActionResult<string>(entity.Text);
		}

		[HttpDelete("Message")]
		public async Task<ActionResult> DeleteMessage()
		{
			var entity = await _context.Message.FirstOrDefaultAsync();
			if (entity == null)
			{
				return NotFound();
			}

			_context.Message.Remove(entity);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
