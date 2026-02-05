using MachinTestForHDFC.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace MachinTestForHDFC.Database
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<EmployeeTaxCalculationDetails> EmployeeTaxCalculationDetails { get; set; }
    }
}
