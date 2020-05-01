using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
    public class VirtualMachineContext :DbContext
    {
        public DbSet<VirtualMachine> VirtualMachines { get; set; }

        public VirtualMachineContext(DbContextOptions<VirtualMachineContext> options)
            :base(options)
        {
        }
    }
}