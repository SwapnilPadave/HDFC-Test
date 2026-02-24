namespace MachinTestForHDFC.Dto.ProductDto
{
    public class UpdateProductDetailsRequestDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
