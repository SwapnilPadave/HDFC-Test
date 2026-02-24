using MachinTestForHDFC.Dto.DepartmentDto;
using MachinTestForHDFC.Models.Department;

namespace MachinTestForHDFC.Services.Department
{
    public interface IDepartmentService
    {
        Task<List<GetDepartmentsDto>> GetAllDepartmentsAsync();
    }
}
