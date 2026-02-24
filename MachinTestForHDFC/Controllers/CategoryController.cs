using MachinTestForHDFC.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace MachinTestForHDFC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetALlCategoriesAsync();
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return Json(result);
        }
    }
}
