using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachinTestForHDFC.Models.CountryStateCity
{
    [Table("EmployeeCountryStateCityMapping")]
    public class EmployeeCountryStateCityMapping
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate {  get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive {  get; set; }
    }
}
