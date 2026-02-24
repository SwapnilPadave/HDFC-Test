using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dto.ProductDto;
using MachinTestForHDFC.Models.Product;
using MachinTestForHDFC.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace MachinTestForHDFC.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly TestDbContext _context;
        public ProductService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetAllProductsDetailsDto>> GetAllProductDetailsAsync(string? searchText, int pageNumber, int pageSize)
        {
            #region Using linq query
            //var query = _context.ProductDetails
            //    .AsNoTracking()
            //    .Where(x => x.IsActive);

            //if (!string.IsNullOrWhiteSpace(searchText))
            //{
            //    query = query.Where(x =>
            //    EF.Functions.Like(x.ProductName, $"%{searchText}%"));
            //}

            //var products = await query
            //    .Join(_context.CategoryDetails,
            //    p => p.CategoryId,
            //    c => c.Id,
            //    (p, c) => new GetAllProductsDetailsDto
            //    {
            //        Id = p.Id,
            //        ProductName = p.ProductName,
            //        CategoryId = p.CategoryId,
            //        CategoryName = c.CategoryName,
            //        Price = p.Price,
            //        TotalQuantity = p.TotalQuantity,
            //        AvailableQuantity = p.AvailableQuantity,
            //        IsActive = p.IsActive,
            //    })
            //    .OrderBy(x => x.ProductName)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToListAsync();

            //return products;
            #endregion

            #region Using EF core navigation property query
            var products = await _context.ProductDetails
                    .AsNoTracking()
                    .Include(p => p.CategoryDetails)
                    .Where(x => x.IsActive)
                    .Where(x => string.IsNullOrWhiteSpace(searchText) ||
                    EF.Functions.Like(x.ProductName, $"%{searchText}%"))
                    .OrderBy(x => x.ProductName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new GetAllProductsDetailsDto
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        CategoryId = p.CategoryId,
                        CategoryName = p.CategoryDetails.CategoryName,
                        Price = p.Price,
                        TotalQuantity = p.TotalQuantity,
                        AvailableQuantity = p.AvailableQuantity,
                        IsActive = p.IsActive,
                    })
                    .ToListAsync();
            return products;
            #endregion
        }

        public async Task<ProductsDetails?> GetProductDetailsByIdAsync(int id)
        {
            var result = await _context.ProductDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<ServiceResult> CreateProductDetailsAsync(CreateProductDetailsRequestDto requestDto)
        {
            var result = new ServiceResult();
            try
            {
                var isCategoryExists = await _context.CategoryDetails.AnyAsync(x => x.Id == requestDto.CategoryId);
                if (!isCategoryExists)
                    result.Errors.Add(("CategoryId", "Category not found."));
                if (result.Errors.Any())
                    return result;

                var product = new ProductsDetails
                {
                    ProductName = requestDto.ProductName,
                    CategoryId = requestDto.CategoryId,
                    Price = requestDto.Price,
                    TotalQuantity = requestDto.TotalQuantity,
                    AvailableQuantity = requestDto.AvailableQuantity,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                await _context.ProductDetails.AddAsync(product);
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Errors = new List<(string Field, string Message)> { ("", "Error while creating product: " + ex.Message) }
                };
            }
        }

        public async Task<ServiceResult> UpdateProductDetailsAsync(int id, UpdateProductDetailsRequestDto requestDto)
        {
            var result = new ServiceResult();
            var product = await _context.ProductDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                result.Errors.Add(("", "Record not found."));
                return result;
            }
            var category = await _context.CategoryDetails.FirstOrDefaultAsync(x => x.Id == requestDto.CategoryId);
            if (category == null)
            {
                result.Errors.Add(("CategoryId", "Category not found."));
            }
            try
            {
                product.ProductName = requestDto.ProductName;
                product.CategoryId = requestDto.CategoryId;
                product.Price = requestDto.Price;
                product.TotalQuantity = requestDto.TotalQuantity;
                product.AvailableQuantity = requestDto.AvailableQuantity;
                product.ModifiedBy = 1;
                product.ModifiedDate = DateTime.UtcNow;
                _context.ProductDetails.Update(product);
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Errors = new List<(string Field, string Message)> { ("", "Error while updating product: " + ex.Message) }
                };
            }
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = new ServiceResult();

            var data = await _context.ProductDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data == null)
            {
                result.Errors.Add(("", "Record not found."));
                return result;
            }

            data.IsActive = false;
            data.ModifiedBy = 1;
            data.ModifiedDate = DateTime.UtcNow;
            _context.ProductDetails.Update(data);
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
