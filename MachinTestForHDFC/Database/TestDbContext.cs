using MachinTestForHDFC.Models.CountryStateCity;
using MachinTestForHDFC.Models.Department;
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
        public DbSet<Departments> Departments { get; set; }
        public DbSet<CountryMaster> CountryMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
        public DbSet<CityMaster> CityMaster { get; set; }
        public DbSet<EmployeeCountryStateCityMapping> EmployeeCountryStateCityMapping { get; set; }
    }
}
