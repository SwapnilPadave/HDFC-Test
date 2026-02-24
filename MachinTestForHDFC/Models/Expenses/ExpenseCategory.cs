using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachinTestForHDFC.Models.Expenses
{
    [Table("ExpenseCategory")]
    public class ExpenseCategory
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
