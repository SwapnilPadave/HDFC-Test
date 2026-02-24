using MachinTestForHDFC.Models.Category;
using MachinTestForHDFC.Models.CountryStateCity;
using MachinTestForHDFC.Models.Department;
using MachinTestForHDFC.Models.Employee;
using MachinTestForHDFC.Models.Product;
using Microsoft.EntityFrameworkCore;
using MachinTestForHDFC.Dto.ProductDto;
using MachinTestForHDFC.Models.Expenses;
using MachinTestForHDFC.Dto.ExpenseDto;

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
        public DbSet<CategoryDetails> CategoryDetails { get; set; }
        public DbSet<ProductsDetails> ProductDetails { get; set; }
        public DbSet<GetAllProductsDetailsDto> GetAllProductsDetailsDto { get; set; } = default!;
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<ExpenseTransactionsRequestsDetails> ExpenseTransactionsRequestsDetails { get; set; }
        public DbSet<MachinTestForHDFC.Dto.ExpenseDto.GetExpenseTransactionDetailsDto> GetExpenseTransactionDetailsDto { get; set; } = default!;
        public DbSet<MachinTestForHDFC.Dto.ExpenseDto.GetExpenseTransactionRequestDetailsDto> GetExpenseTransactionRequestDetailsDto { get; set; } = default!;
    }
}
