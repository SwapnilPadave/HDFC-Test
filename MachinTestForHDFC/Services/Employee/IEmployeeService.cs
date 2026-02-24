using MachinTestForHDFC.Dto.EmployeeDto;
using MachinTestForHDFC.Models.Employee;
using MachinTestForHDFC.ResponseModels;

namespace MachinTestForHDFC.Services.Employee
{
    public interface IEmployeeService
    {
        Task<List<Employees>> GetAllEmployeesAsync();
        Task<GetEmployeesDetailsById?> GetByIdAsync(int id);
        Task<ServiceResult> CreateEmployeeAsync(CreateEmployeeRequestDto request);
        Task<ServiceResult> UpdateEmployeeAsync(int id, UpdateEmployeeRequestDto request);
        decimal CalculateTax(decimal salary);
        Task<int> GenerateEmployeeCodeAsync();
        Task<bool> CheckDuplicateEmpCodeAsync(int empCode, int? id);
    }
}
