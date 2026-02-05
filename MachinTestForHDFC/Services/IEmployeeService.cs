using MachinTestForHDFC.Dtos;
using MachinTestForHDFC.Models.Employee;

namespace MachinTestForHDFC.Services
{
    public interface IEmployeeService
    {
        Task<List<Employees>> GetAllEmployeesAsync();
        Task<Employees?> GetByIdAsync(int id);
        Task<Employees> CreateEmployeeAsync(CreateEmployeeRequestDto request);
        Task<Employees> UpdateEmployeeAsync(int id, UpdateEmployeeRequestDto request);
        decimal CalculateTax(decimal salary);
        Task<int> GenerateEmployeeCodeAsync();
    }
}
