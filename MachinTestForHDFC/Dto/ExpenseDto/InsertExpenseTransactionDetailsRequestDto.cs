namespace MachinTestForHDFC.Dto.ExpenseDto
{
    public class InsertExpenseTransactionDetailsRequestDto
    {
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateExpenseTransactionDetailsRequestDto
    {
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
