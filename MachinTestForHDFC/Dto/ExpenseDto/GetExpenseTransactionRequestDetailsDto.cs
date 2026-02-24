namespace MachinTestForHDFC.Dto.ExpenseDto
{
    public class GetExpenseTransactionRequestDetailsDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal ExpenseAmount { get; set; }
        public DateTime? FromDate {  get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ActionBy { get; set; } = string.Empty;
        public DateTime? ActionDate { get; set; }
        public string Remark { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
