using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dto.EmployeeDto;
using MachinTestForHDFC.Models.Employee;
using MachinTestForHDFC.ResponseModels;
using MachinTestForHDFC.Services.CountryStateCity;
using MachinTestForHDFC.Services.Department;
using MachinTestForHDFC.Services.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace MachinTestForHDFC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly TestDbContext _context;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly ICountryStateCityService _countryStateCityService;
        private readonly HttpClient _httpClient;

        public EmployeesController(TestDbContext context, IEmployeeService employeeService, HttpClient httpClient, IDepartmentService departmentService, ICountryStateCityService countryStateCityService)
        {
            _context = context;
            _employeeService = employeeService;
            _httpClient = httpClient;
            _departmentService = departmentService;
            _countryStateCityService = countryStateCityService;
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
                if (!result.IsSuccess)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Field, error.Message);
                    }
                    return View(requestDto);
                }
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
                if (!result.IsSuccess)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Field, error.Message);
                    }
                    return View(employees);
                }
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

        [HttpGet]
        public async Task<IActionResult> CheckDuplicateEmpCode(int empCode, int? id)
        {
            var result = await _employeeService.CheckDuplicateEmpCodeAsync(empCode, id);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateByUrl()
        {
            #region Hardcoded data for testing.
            //string response = "{\r\n  \"statusCode\": 200,\r\n  \"message\": \"Success.\",\r\n  \"data\": {\r\n    \"name\": \"Swapnil Padave\",\r\n    \"email\": \"Swapi@Gmail.Com\",\r\n    \"mobileNo\": \"9876543210\",\r\n    \"address\": \"Lalbaug\",\r\n    \"departmentId\": 1,\r\n    \"countryId\": 72,\r\n    \"stateId\": 14,\r\n    \"cityId\": 66\r\n  },\r\n  \"errors\": []\r\n}";

            //var apiResponse = JsonSerializer.Deserialize<ApiResponse<EmployeeDto>>(response);
            //if(apiResponse == null || apiResponse.data == null)
            //    return View(new AddEmployeeViewModel()); 
            #endregion

            #region From api call.
            //var url = "https://localhost:7295/api/Employee/TestAddEmployeeByUrl";

            //var array = url.Split('?');
            //var baseUrl = array[0];
            //var apiResponse = array[1];

            //var response = await _httpClient.GetAsync(url);
            //if (!response.IsSuccessStatusCode)
            //    return Content("Api call fail.");

            //var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            //if (apiResponse == null || apiResponse.data == null)
            //    return Content("Invalid API response.");

            //var decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(apiResponse.data)); 
            #endregion

            #region For hardcoded api response.
            var url = "https://localhost:7295/api/Employee/TestAddEmployeeByUrl?bmFtZT1Td2FwbmlsJGVtYWlsPWFiY0BnbWFpbC5jb20kbW9iaWxlTm89OTg3NjU0NjM1MiRhZGRyZXNzPVRlc3QgQWRkcmVzcyRkZXBhcnRtZW50SWQ9MSRjb3VudHJ5SWQ9NzIkc3RhdGVJZD0xNCRjaXR5SWQ9NjY=";

            var array = url.Split('?');
            var baseUrl = array[0];
            var apiResponse = array[1]; 
            #endregion

            var decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(apiResponse));

            #region Using dictionary to map the data to dto.
            var strArray = decodedString.Split('$');

            var parameters = new Dictionary<string, string>();

            foreach (var item in strArray)
            {
                if (item.Contains('='))
                {
                    var keyValue = item.Split('=');
                    if (keyValue.Length == 2)
                    {
                        parameters[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                }
            }

            var model = new AddEmployeeViewModel();

            model.Name = parameters.TryGetValue("name", out var name) ? name : "";
            model.Email = parameters.ContainsKey("email") ? parameters["email"] : "";
            model.MobileNo = parameters.TryGetValue("mobileNo", out var mobileNo) ? mobileNo : "";
            model.Address = parameters.TryGetValue("address", out var address) ? address : "";
            model.DepartmentId = parameters.TryGetValue("departmentId", out var departmentId) ? Convert.ToInt32(departmentId) : 0;
            model.CountryId = parameters.TryGetValue("countryId", out var countryId) ? Convert.ToInt32(countryId) : 0;
            model.StateId = parameters.TryGetValue("stateId", out var stateId) ? Convert.ToInt32(stateId) : 0;
            model.CityId = parameters.TryGetValue("cityId", out var cityId) ? Convert.ToInt32(cityId) : 0;
            #endregion

            #region Using switch to map the data to dto.
            //var dto = new AddEmployeeViewModel();

            //foreach (var item in decodedString.Split('$'))
            //{
            //    var keyValue = item.Split('=');
            //    if (keyValue.Length != 2)
            //        continue;

            //    var key = keyValue[0];
            //    var value = keyValue[1];

            //    switch (key)
            //    {
            //        case "name": dto.Name = value; break;
            //        case "email": dto.Email = value; break;
            //        case "mobileNo": dto.MobileNo = value; break;
            //        case "address": dto.Address = value; break;
            //        case "departmentId": dto.DepartmentId = int.Parse(value); break;
            //        case "countryId": dto.CountryId = int.Parse(value); break;
            //        case "stateId": dto.StateId = int.Parse(value); break;
            //        case "cityId": dto.CityId = int.Parse(value); break;
            //    }
            //} 
            #endregion

            #region Direct mapping data to the dto.
            //var model = new AddEmployeeViewModel
            //{
            //    Name = apiResponse.data.name,
            //    Email = apiResponse.data.email,
            //    MobileNo = apiResponse.data.mobileNo,
            //    Address = apiResponse.data.address,
            //    DepartmentId = apiResponse.data.departmentId,
            //    CountryId = apiResponse.data.countryId,
            //    StateId = apiResponse.data.stateId,
            //    CityId = apiResponse.data.cityId
            //}; 
            #endregion

            await LoadDropdown(model);
            //await LoadDropdown(dto);

            return View("CreateByUrl", model);
            //return View("CreateByUrl", dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateByUrl(AddEmployeeViewModel requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.CreateEmployeeByUrlAsync(requestDto);
                if (!result.IsSuccess)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Field, error.Message);
                    }
                    return View(requestDto);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(requestDto);
        }

        private async Task LoadDropdown(AddEmployeeViewModel model)
        {
            var department = await _departmentService.GetAllDepartmentsAsync();

            model.Departments = department!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = d.Id == model.DepartmentId
            }).ToList();

            var country = await _countryStateCityService.GetAllCountryAsync();

            model.Countries = country!.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CountryName,
                Selected = c.Id == model.CountryId
            }).ToList();

            var state = await _countryStateCityService.GetAllStatesByCountryIdAsync(model.CountryId);

            model.States = state!.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.StateName,
                Selected = c.Id == model.StateId
            }).ToList();

            var city = await _countryStateCityService.GetAllCityByStateIdAsync(model.StateId);

            model.Cities = city!.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CityName,
                Selected = c.Id == model.CityId
            }).ToList();
        }
    }
}