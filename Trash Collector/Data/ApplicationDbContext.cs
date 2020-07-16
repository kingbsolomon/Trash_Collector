using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trash_Collector.Models;

namespace Trash_Collector.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Trash_Collector.Models.Employee> Employee { get; set; }
        public DbSet<Trash_Collector.Models.Customer> Customer { get; set; }
    }
}
