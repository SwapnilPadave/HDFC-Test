using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachinTestForHDFC.Models.Expenses
{
    [Table("ExpenseTransactionsRequestsDetails")]
    public class ExpenseTransactionsRequestsDetails
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public char Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ActionBy { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Remark { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
