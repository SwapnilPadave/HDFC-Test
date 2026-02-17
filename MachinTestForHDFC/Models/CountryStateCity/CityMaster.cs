using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachinTestForHDFC.Models.CountryStateCity
{
    [Table("CityMaster")]
    public class CityMaster
    {
        [Key]
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int StateId { get; set; }
    }
}
