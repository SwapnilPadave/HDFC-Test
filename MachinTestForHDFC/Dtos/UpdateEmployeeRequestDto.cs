namespace MachinTestForHDFC.Dtos
{
    public class UpdateEmployeeRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public int EmpCode { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
        public DateTime DOB { get; set; }
    }
}
