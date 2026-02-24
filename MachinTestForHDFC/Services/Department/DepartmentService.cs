using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dto.DepartmentDto;
using Microsoft.EntityFrameworkCore;

namespace MachinTestForHDFC.Services.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly TestDbContext _context;
        public DepartmentService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetDepartmentsDto>> GetAllDepartmentsAsync()
        {
            var data = await _context.Departments
                .Select(x => new GetDepartmentsDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return data;
        }
    }
}
