namespace MachinTestForHDFC.Dtos
{
    public class GetEmployeesDetailsById
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int EmpCode { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
        public DateTime DOB { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
    }
}
