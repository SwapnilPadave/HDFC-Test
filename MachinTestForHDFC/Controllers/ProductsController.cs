using MachinTestForHDFC.Dto.ProductDto;
using MachinTestForHDFC.Services.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MachinTestForHDFC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchText, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productService.GetAllProductDetailsAsync(searchText, pageNumber, pageSize);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _productService.GetProductDetailsByIdAsync(id);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateProductDetailsRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductDetailsAsync(requestDto);
                if (!result.IsSuccess)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Field, error.Message);
                    }
                    return View(requestDto);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(requestDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductDetailsByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductDetailsRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductDetailsAsync(id, requestDto);
                if (!result.IsSuccess)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Field, error.Message);
                    }
                    return View(requestDto);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(requestDto);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result.IsSuccess)
                return Ok("Record deleted successfully.");
            return BadRequest(result.Errors.FirstOrDefault());
        }
    }
}
