using MachinTestForHDFC.Models.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachinTestForHDFC.Models.Product
{
    [Table("ProductsDetails")]
    public class ProductsDetails
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryDetails CategoryDetails { get; set; }
    }
}
