using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Factory.Models
{
    public class FactoryContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Engineer> Engineers { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<EngineerMachine> EngineerMachine { get; set; }

        public FactoryContext(DbContextOptions options) : base(options) { }
    }
}