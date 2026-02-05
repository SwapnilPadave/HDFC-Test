using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dtos;
using MachinTestForHDFC.Services;
using Microsoft.AspNetCore.Mvc;

namespace MachinTestForHDFC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly TestDbContext _context;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(TestDbContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employees = await _employeeService.GetByIdAsync(id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.CreateEmployeeAsync(requestDto);
                return RedirectToAction(nameof(Index));
            }
            return View(requestDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employees = await _employeeService.GetByIdAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            return View(employees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEmployeeRequestDto employees)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.UpdateEmployeeAsync(id, employees);
                return RedirectToAction(nameof(Index));
            }
            return View(employees);
        }

        [HttpPost]
        public IActionResult CalculateTax(decimal salary)
        {
            var tax = _employeeService.CalculateTax(salary);
            return Json(tax);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateEmpCode()
        {
            var code = await _employeeService.GenerateEmployeeCodeAsync();
            return Json(code);
        }
    }
}
