namespace MachinTestForHDFC.Dto.ExpenseDto
{
    public class GetExpenseTransactionDetailsDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
    }

    public class GetExpenseTransactionById
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
