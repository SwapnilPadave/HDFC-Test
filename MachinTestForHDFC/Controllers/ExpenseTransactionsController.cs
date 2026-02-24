using MachinTestForHDFC.Dto.ExpenseDto;
using MachinTestForHDFC.Services.Expenses;
using Microsoft.AspNetCore.Mvc;

namespace MachinTestForHDFC.Controllers
{
    public class ExpenseTransactionsController : Controller
    {
        private readonly IExpenseTransactionService _expenseTransactionService;
        public ExpenseTransactionsController(IExpenseTransactionService expenseTransactionService)
        {
            _expenseTransactionService = expenseTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var data = await _expenseTransactionService.GetAllCategoriesAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await _expenseTransactionService.GetAllEmployeesAsync();
            return Json(result);
        }

        public IActionResult InsertExpenseTransactionDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertExpenseTransactionDetails(InsertExpenseTransactionDetailsRequestDto requestDto)
        {
            var result = await _expenseTransactionService.InsertExpenseTransactionsAsync(requestDto);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Field, error.Message);
                    return View(requestDto);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenseTransactionData(string? searchText, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _expenseTransactionService.GetAllExpenseTransactionRequestDetailsAsync(searchText, pageNumber, pageSize);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _expenseTransactionService.GetExpenseTransactionDetailsByIdForUpdate(id);
            if (data == null)
                return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateExpenseTransactionDetailsRequestDto requestDto)
        {
            var result = await _expenseTransactionService.UpdateExpenseTransactionDetailsAsync(id, requestDto);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Field, error.Message);
                    return View(requestDto);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTransactionStatus(int id, char status, string? remark)
        {
            var data = await _expenseTransactionService.UpdateExpenseTransactionStatusAsync(id, status, remark);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _expenseTransactionService.GetTransactionRequestById(id);
            if (result == null)
                return NotFound();
            return View(result);
        }
    }
}
