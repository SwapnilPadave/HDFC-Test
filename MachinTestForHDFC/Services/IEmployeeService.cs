using MachinTestForHDFC.Dtos;
using MachinTestForHDFC.Models.Employee;
using MachinTestForHDFC.ResponseModels;

namespace MachinTestForHDFC.Services
{
    public interface IEmployeeService
    {
        Task<List<Employees>> GetAllEmployeesAsync();
        Task<Employees?> GetByIdAsync(int id);
        Task<ServiceResult> CreateEmployeeAsync(CreateEmployeeRequestDto request);
        Task<Employees> UpdateEmployeeAsync(int id, UpdateEmployeeRequestDto request);
        decimal CalculateTax(decimal salary);
        Task<int> GenerateEmployeeCodeAsync();
        Task<bool> CheckDuplicateEmpCodeAsync(int empCode, int? id);
    }
}
