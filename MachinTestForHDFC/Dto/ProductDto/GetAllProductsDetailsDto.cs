namespace MachinTestForHDFC.Dto.ProductDto
{
    public class GetAllProductsDetailsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
