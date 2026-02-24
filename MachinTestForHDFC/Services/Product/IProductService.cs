using MachinTestForHDFC.Dto.ProductDto;
using MachinTestForHDFC.Models.Product;
using MachinTestForHDFC.ResponseModels;

namespace MachinTestForHDFC.Services.Product
{
    public interface IProductService
    {
        Task<List<GetAllProductsDetailsDto>> GetAllProductDetailsAsync(string? searchText, int pageNumber, int pageSize);
        Task<ProductsDetails?> GetProductDetailsByIdAsync(int id);
        Task<ServiceResult> CreateProductDetailsAsync(CreateProductDetailsRequestDto requestDto);
        Task<ServiceResult> UpdateProductDetailsAsync(int id, UpdateProductDetailsRequestDto requestDto);
        Task<ServiceResult> DeleteProductAsync(int id);
    }
}
