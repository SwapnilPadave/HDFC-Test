using MachinTestForHDFC.Models.Category;

namespace MachinTestForHDFC.Services.Category
{
    public interface ICategoryService
    {
        Task<List<CategoryDetails>> GetALlCategoriesAsync();
        Task<CategoryDetails?> GetCategoryByIdAsync(int id);
    }
}
