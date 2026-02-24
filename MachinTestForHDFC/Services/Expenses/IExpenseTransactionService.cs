using MachinTestForHDFC.Dto.EmployeeDto;
using MachinTestForHDFC.Dto.ExpenseDto;
using MachinTestForHDFC.Models.Expenses;
using MachinTestForHDFC.ResponseModels;

namespace MachinTestForHDFC.Services.Expenses
{
    public interface IExpenseTransactionService
    {
        Task<List<ExpenseCategory>> GetAllCategoriesAsync();
        Task<List<GetEmployeesForDropdownDto>> GetAllEmployeesAsync();
        Task<GetExpenseTransactionById?> GetExpenseTransactionDetailsByIdForUpdate(int id);
        Task<ServiceResult> InsertExpenseTransactionsAsync(InsertExpenseTransactionDetailsRequestDto requestDto);
        Task<PagedResult<GetExpenseTransactionDetailsDto>> GetAllExpenseTransactionRequestDetailsAsync(string? searchText, int pageNumber, int pageSize);
        Task<ServiceResult> UpdateExpenseTransactionDetailsAsync(int id, UpdateExpenseTransactionDetailsRequestDto requestDto);
        Task<string> UpdateExpenseTransactionStatusAsync(int id, char status, string? remark);
        Task<GetExpenseTransactionRequestDetailsDto?> GetTransactionRequestById(int id);
    }
}
