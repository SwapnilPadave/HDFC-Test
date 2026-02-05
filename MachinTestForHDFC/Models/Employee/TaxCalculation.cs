using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachinTestForHDFC.Models.Employee
{
    [Table("EmployeeTaxCalculationDetails")]
    public class EmployeeTaxCalculationDetails
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TaxDeductedSalary { get; set; }
        public decimal TaxPercent { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
