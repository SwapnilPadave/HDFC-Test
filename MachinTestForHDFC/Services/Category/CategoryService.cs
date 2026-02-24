using MachinTestForHDFC.Database;
using MachinTestForHDFC.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace MachinTestForHDFC.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly TestDbContext _dbContext;
        public CategoryService(TestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryDetails>> GetALlCategoriesAsync()
        {
            var data = await _dbContext.CategoryDetails.Where(x => x.IsActive == true).ToListAsync();
            return data;
        }

        public async Task<CategoryDetails?> GetCategoryByIdAsync(int id)
        {
            var data = await _dbContext.CategoryDetails.Where(x=>x.Id== id).FirstOrDefaultAsync();
            return data;
        }
    }
}
