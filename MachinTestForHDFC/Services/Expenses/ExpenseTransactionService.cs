using Dapper;
using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dto.EmployeeDto;
using MachinTestForHDFC.Dto.ExpenseDto;
using MachinTestForHDFC.Models.Expenses;
using MachinTestForHDFC.ResponseModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MachinTestForHDFC.Services.Expenses
{
    public class ExpenseTransactionService : IExpenseTransactionService
    {
        private readonly TestDbContext _context;
        public ExpenseTransactionService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseCategory>> GetAllCategoriesAsync()
        {
            var data = await _context.ExpenseCategories
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            return data;
        }

        public async Task<GetExpenseTransactionById?> GetExpenseTransactionDetailsByIdForUpdate(int id)
        {
            var data = await _context.ExpenseTransactionsRequestsDetails
                    .Where(x => x.Id == id)
                    .Select(x => new GetExpenseTransactionById
                    {
                        Id = x.Id,
                        EmployeeId = x.EmployeeId,
                        CategoryId = x.CategoryId,
                        ExpenseAmount = x.ExpenseAmount,
                        FromDate = x.FromDate,
                        ToDate = x.ToDate,
                        Description = x.Description
                    })
                    .FirstOrDefaultAsync();

            return data;
        }

        public async Task<List<GetEmployeesForDropdownDto>> GetAllEmployeesAsync()
        {
            var data = await _context.Employees
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => new GetEmployeesForDropdownDto
                {
                    Id = x.Id,
                    FullName = x.FullName
                }).ToListAsync();
            return data;
        }

        public async Task<ServiceResult> InsertExpenseTransactionsAsync(InsertExpenseTransactionDetailsRequestDto requestDto)
        {
            var result = new ServiceResult();
            try
            {
                var fromDate = requestDto.FromDate.Date;
                var toDate = requestDto.ToDate.Date;

                using var connection = _context.Database.GetDbConnection();
                var param = new DynamicParameters();
                param.Add("@EmployeeId", requestDto.EmployeeId);
                param.Add("@CategoryId", requestDto.CategoryId);
                param.Add("@ExpenseAmount", requestDto.ExpenseAmount);
                param.Add("@FromDate", fromDate);
                param.Add("@ToDate", toDate);
                param.Add("@Description", requestDto.Description);
                param.Add("@LoggedInUserId", 1);

                var data = await connection.QueryFirstOrDefaultAsync<SPResponseResult>(
                    "Usp_InsertExpenseTransactionDetails",
                    param,
                    commandType: CommandType.StoredProcedure
                    );

                if (data?.Status == 1)
                {
                    return result;
                }

                result.Errors.Add(("", $"Error while inserting details: {data?.Message}"));
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(("", ex.Message));
                return result;
            }
        }

        public async Task<PagedResult<GetExpenseTransactionDetailsDto>> GetAllExpenseTransactionRequestDetailsAsync(string? searchText, int pageNumber, int pageSize)
        {
            var query = from exp in _context.ExpenseTransactionsRequestsDetails
                        join cat in _context.ExpenseCategories on exp.CategoryId equals cat.Id
                        join emp in _context.Employees on exp.EmployeeId equals emp.Id
                        select new
                        {
                            exp.Id,
                            emp.FullName,
                            cat.CategoryName,
                            exp.ExpenseAmount,
                            exp.FromDate,
                            exp.ToDate,
                            exp.Status,
                            exp.Description
                        };

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.FullName, $"%{searchText}%") ||
                    EF.Functions.Like(x.CategoryName, $"%{searchText}%"));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x =>
                new GetExpenseTransactionDetailsDto
                {
                    Id = x.Id,
                    EmployeeName = x.FullName,
                    CategoryName = x.CategoryName,
                    ExpenseAmount = x.ExpenseAmount,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Status = x.Status == 'P' ? "Pending" :
                             x.Status == 'A' ? "Approved" : "Rejected",
                    Description = x.Description
                })
                .ToListAsync();

            return new PagedResult<GetExpenseTransactionDetailsDto>
            {
                Data = data,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }

        public async Task<ServiceResult> UpdateExpenseTransactionDetailsAsync(int id, UpdateExpenseTransactionDetailsRequestDto requestDto)
        {
            var result = new ServiceResult();

            try
            {
                var fromDate = Convert.ToDateTime(requestDto.FromDate.ToString());
                var toDate = Convert.ToDateTime(requestDto.ToDate.ToString());

                using var connection = _context.Database.GetDbConnection();
                var param = new DynamicParameters();
                param.Add("@TransactionId", id);
                param.Add("@EmployeeId", requestDto.EmployeeId);
                param.Add("@CategoryId", requestDto.CategoryId);
                param.Add("@ExpenseAmount", requestDto.ExpenseAmount);
                param.Add("@FromDate", fromDate);
                param.Add("@ToDate", toDate);
                param.Add("@Description", requestDto.Description);
                param.Add("@LoggedInUserId", 1);

                var data = await connection.QueryFirstOrDefaultAsync<SPResponseResult>(
                    "Usp_UpdateExpenseTransactionDetails",
                    param,
                    commandType: CommandType.StoredProcedure
                    );

                if (data?.Status == 1)
                {
                    return result;
                }

                result.Errors.Add(("", $"Error while updating details: {data?.Message}"));
                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add(("", ex.Message));
                return result;
            }
        }

        public async Task<string> UpdateExpenseTransactionStatusAsync(int id, char status, string? remark)
        {
            string responseMessage = string.Empty;
            var data = await _context.ExpenseTransactionsRequestsDetails
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (data != null)
            {
                switch (status)
                {
                    case ('A'):
                        data.Status = status;
                        data.Remark = remark ?? null;
                        data.ActionBy = 1;
                        data.ActionDate = DateTime.UtcNow;
                        data.IsActive = false;
                        _context.ExpenseTransactionsRequestsDetails.Update(data);
                        await _context.SaveChangesAsync();
                        responseMessage = "Record approved successfully.";
                        break;
                    case ('R'):
                        data.Status = status;
                        data.Remark = remark ?? null;
                        data.ActionBy = 1;
                        data.ActionDate = DateTime.UtcNow;
                        data.IsActive = false;
                        _context.ExpenseTransactionsRequestsDetails.Update(data);
                        await _context.SaveChangesAsync();
                        responseMessage = "Record rejected successfully.";
                        break;
                    case ('P'):
                        data.Status = status;
                        data.Remark = remark ?? null;
                        data.ActionBy = 1;
                        data.ActionDate = DateTime.UtcNow;
                        data.IsActive = true;
                        _context.ExpenseTransactionsRequestsDetails.Update(data);
                        await _context.SaveChangesAsync();
                        responseMessage = "Record status changed to pending.";
                        break;
                    default:
                        responseMessage = "Invalid status.";
                        break;
                }
                return responseMessage;
            }
            responseMessage = "Record not found.";
            return responseMessage;
        }

        public async Task<GetExpenseTransactionRequestDetailsDto?> GetTransactionRequestById(int id)
        {
            var data = await (from exp in _context.ExpenseTransactionsRequestsDetails
                              join emp in _context.Employees on exp.EmployeeId equals emp.Id
                              join cat in _context.ExpenseCategories on exp.CategoryId equals cat.Id
                              join act in _context.Employees on exp.ActionBy equals act.Id into actionJoin
                              from act in actionJoin.DefaultIfEmpty()
                              where exp.Id == id
                              select new GetExpenseTransactionRequestDetailsDto
                              {
                                  Id = exp.Id,
                                  EmployeeName = emp.FullName,
                                  CategoryName = cat.CategoryName,
                                  ExpenseAmount = exp.ExpenseAmount,
                                  FromDate = exp.FromDate,
                                  ToDate = exp.ToDate,
                                  AppliedDate = exp.CreatedDate,
                                  Description = exp.Description,
                                  Status = exp.Status == 'A' ? "Approved" : exp.Status == 'P' ? "Pending" : "Rejected",
                                  ActionBy = act != null ? act.FullName : "",
                                  ActionDate = exp.ActionDate,
                                  Remark = exp.Remark ?? "",
                                  IsActive = exp.IsActive
                              })
                              .FirstOrDefaultAsync();
            return data;
        }
    }
}
