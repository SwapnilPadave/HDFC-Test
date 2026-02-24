using Dapper;
using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dto.EmployeeDto;
using MachinTestForHDFC.Models.Employee;
using MachinTestForHDFC.ResponseModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MachinTestForHDFC.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TestDbContext _context;
        public EmployeeService(TestDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employees>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<GetEmployeesDetailsById?> GetByIdAsync(int id)
        {
            var data = await (from e in _context.Employees
                              join ecsc in _context.EmployeeCountryStateCityMapping
                              on e.Id equals ecsc.EmployeeId into mappings
                              from ecsc in mappings.DefaultIfEmpty()
                              where e.Id == id
                              && (ecsc == null || ecsc.IsActive == true)
                              select new GetEmployeesDetailsById
                              {
                                  Id = e.Id,
                                  FullName = e.FullName,
                                  EmpCode = e.EmpCode,
                                  DepartmentId = e.DepartmentId,
                                  Salary = e.Salary,
                                  DOB = e.DOB,
                                  CountryId = ecsc != null ? ecsc.CountryId : null,
                                  StateId = ecsc != null ? ecsc.StateId : null,
                                  CityId = ecsc != null ? ecsc.CityId : null
                              }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ServiceResult> CreateEmployeeAsync(CreateEmployeeRequestDto request)
        {
            var result = new ServiceResult();
            var isExists = await _context.Employees.AnyAsync(x => x.EmpCode == request.EmpCode);
            if (isExists)
                result.Errors.Add(("EmpCode", "Employee Code already exists."));

            if (result.Errors.Any())
                return result;

            #region Using EF Core
            //var transaction = await _context.Database.BeginTransactionAsync();
            //try
            //{
            //    var employee = new Employees
            //    {
            //        FullName = request.FullName,
            //        EmpCode = request.EmpCode,
            //        DepartmentId = request.DepartmentId,
            //        Salary = request.Salary,
            //        DOB = request.DOB,
            //        CreatedBy = 1,
            //        CreatedDate = DateTime.UtcNow,
            //        IsActive = true
            //    };
            //    var data = await _context.Employees.AddAsync(employee);
            //    await _context.SaveChangesAsync();

            //    var employeeTaxCalculation = new EmployeeTaxCalculationDetails
            //    {
            //        EmployeeId = data.Entity.Id,
            //        BasicSalary = request.Salary,
            //        TaxDeductedSalary = CalculateTax(request.Salary),
            //        TaxPercent = GetTaxPercent(request.Salary),
            //        CreatedBy = 1,
            //        CreatedDate = DateTime.UtcNow,
            //        IsActive = true
            //    };
            //    await _context.EmployeeTaxCalculationDetails.AddAsync(employeeTaxCalculation);

            //    var countryStateCityMappings = new EmployeeCountryStateCityMapping
            //    {
            //        EmployeeId = data.Entity.Id,
            //        CountryId = request.CountryId,
            //        StateId = request.StateId,
            //        CityId = request.CityId,
            //        CreatedBy = 1,
            //        CreatedDate = DateTime.UtcNow,
            //        IsActive = true
            //    };
            //    await _context.EmployeeCountryStateCityMapping.AddAsync(countryStateCityMappings);
            //    await _context.SaveChangesAsync();
            //    await transaction.CommitAsync();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    return new ServiceResult
            //    {
            //        Errors = new List<(string Field, string Message)> { ("", "Error while creating employee: " + ex.Message) }
            //    };
            //} 
            #endregion

            using var connection = _context.Database.GetDbConnection();

            var taxDeduction = CalculateTax(request.Salary);
            var taxPercent = GetTaxPercent(request.Salary);

            var param = new DynamicParameters();
            param.Add("@FullName", request.FullName);
            param.Add("@EmpCode", request.EmpCode);
            param.Add("@countryId", request.CountryId);
            param.Add("@StateId", request.StateId);
            param.Add("@CityId", request.CityId);
            param.Add("@DepartmentId", request.DepartmentId);
            param.Add("@Salary", request.Salary);
            param.Add("@DOB", request.DOB);
            param.Add("@TaxDeduction", taxDeduction);
            param.Add("@TaxPercent", taxPercent);
            param.Add("@LoggedInUserId", 1);

            var data = await connection.QueryFirstOrDefaultAsync<string>("Usp_InsertEmployeeDetails", param, commandType: CommandType.StoredProcedure);
            if (data == "Success")
                return result;
            return new ServiceResult
            {
                Errors = new List<(string Field, string Message)> { ("", "Error while creating employee: " + result) }
            };
        }

        public async Task<ServiceResult> UpdateEmployeeAsync(int id, UpdateEmployeeRequestDto request)
        {
            var result = new ServiceResult();
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                result.Errors.Add(("", "Record not found."));
                return result;
            }

            var isDuplicate = await _context.Employees.AnyAsync(x => x.EmpCode == request.EmpCode && x.Id != id);
            if (isDuplicate)
                result.Errors.Add(("EmpCode", "Employee Code already exists."));

            if (result.Errors.Any())
                return result;

            #region Using EF Core
            //var transaction = await _context.Database.BeginTransactionAsync();
            //try
            //{
            //    var employeeTaxDetails = await _context.EmployeeTaxCalculationDetails
            //        .FirstOrDefaultAsync(x => x.EmployeeId == id);
            //    var employeeCountryStateCityMappings = await _context.EmployeeCountryStateCityMapping.Where(x => x.EmployeeId == id && x.IsActive == true).FirstOrDefaultAsync();

            //    employee.FullName = request.FullName;
            //    employee.EmpCode = request.EmpCode;
            //    employee.DepartmentId = request.DepartmentId;
            //    employee.Salary = request.Salary;
            //    employee.DOB = request.DOB;
            //    employee.ModifiedBy = 1;
            //    employee.ModifiedDate = DateTime.UtcNow;
            //    _context.Employees.Update(employee);

            //    if (employeeTaxDetails != null)
            //    {
            //        employeeTaxDetails.BasicSalary = request.Salary;
            //        employeeTaxDetails.TaxDeductedSalary = CalculateTax(request.Salary);
            //        employeeTaxDetails.TaxPercent = GetTaxPercent(request.Salary);
            //        employeeTaxDetails.ModifiedBy = 1;
            //        employeeTaxDetails.ModifiedDate = DateTime.UtcNow;
            //        _context.EmployeeTaxCalculationDetails.Update(employeeTaxDetails);
            //    }

            //    if (employeeCountryStateCityMappings != null)
            //    {
            //        employeeCountryStateCityMappings.CountryId = request.CountryId;
            //        employeeCountryStateCityMappings.StateId = request.CountryId;
            //        employeeCountryStateCityMappings.CityId = request.CityId;
            //        employeeCountryStateCityMappings.ModifiedBy = 1;
            //        employeeCountryStateCityMappings.ModifiedDate = DateTime.UtcNow;
            //        _context.EmployeeCountryStateCityMapping.Update(employeeCountryStateCityMappings);
            //    }

            //    await _context.SaveChangesAsync();
            //    await transaction.CommitAsync();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    return new ServiceResult
            //    {
            //        Errors = new List<(string Field, string Message)> { ("", "Error while updating employee: " + ex.Message) }
            //    };
            //} 
            #endregion

            using var connection = _context.Database.GetDbConnection();

            var taxDeduction = CalculateTax(request.Salary);
            var taxPercent = GetTaxPercent(request.Salary);

            var param = new DynamicParameters();
            param.Add("@EmployeeId", id);
            param.Add("@FullName", request.FullName);
            param.Add("@EmpCode", request.EmpCode);
            param.Add("@countryId", request.CountryId);
            param.Add("@StateId", request.StateId);
            param.Add("@CityId", request.CityId);
            param.Add("@DepartmentId", request.DepartmentId);
            param.Add("@Salary", request.Salary);
            param.Add("@DOB", request.DOB);
            param.Add("@TaxDeduction", taxDeduction);
            param.Add("@TaxPercent", taxPercent);
            param.Add("@LoggedInUserId", 1);

            var data = await connection.QueryFirstOrDefaultAsync<string>("Usp_UpdateEmployeeDetails", param, commandType: CommandType.StoredProcedure);
            if (data == "Success")
                return result;
            return new ServiceResult
            {
                Errors = new List<(string Field, string Message)> { ("", "Error while creating employee: " + result) }
            };
        }
        public async Task<int> GenerateEmployeeCodeAsync()
        {
            var lastCode = await _context.Employees
                .OrderByDescending(x => x.EmpCode)
                .Select(x => x.EmpCode)
                .FirstOrDefaultAsync();

            if (lastCode == 0)
                return 1001;

            return lastCode + 1;
        }

        public decimal CalculateTax(decimal salary)
        {
            var result = CalculateSalaryTax(salary);
            return result;
        }

        public async Task<bool> CheckDuplicateEmpCodeAsync(int empCode, int? id)
        {


            var isExists = await _context.Employees.AnyAsync(x => x.EmpCode == empCode && x.Id != id);
            return isExists;
        }

        private decimal CalculateSalaryTax(decimal salary)
        {
            decimal totalTaxDeduction = 0;
            switch (salary)
            {
                case <= 500000:
                    totalTaxDeduction = salary * 5 / 100;
                    return totalTaxDeduction;
                case > 500000 and <= 700000:
                    totalTaxDeduction += 500000 * 5 / 100;
                    totalTaxDeduction += (salary - 500000) * 7 / 100;
                    return totalTaxDeduction;
                case > 700000 and <= 1000000:
                    totalTaxDeduction += 500000 * 5 / 100;
                    totalTaxDeduction += 200000 * 7 / 100;
                    totalTaxDeduction += (salary - 700000) * 10 / 100;
                    return totalTaxDeduction;
                default:
                    totalTaxDeduction += 500000 * 5 / 100;
                    totalTaxDeduction += 200000 * 7 / 100;
                    totalTaxDeduction += 300000 * 10 / 100;
                    totalTaxDeduction = (salary - 1000000) * 12 / 100;
                    return totalTaxDeduction;
            }
            #region If else conditions.
            //if (salary <= 500000)
            //{
            //    decimal totalTaxDeduction = 0;
            //    totalTaxDeduction = salary * 5 / 100;
            //    return totalTaxDeduction;
            //}
            //else if (salary > 500000 && salary <= 700000)
            //{
            //    decimal totalTaxDeduction = 0;
            //    totalTaxDeduction += 500000 * 5 / 100;
            //    totalTaxDeduction += (salary - 500000) * 7 / 100;
            //    return totalTaxDeduction;
            //}
            //else if (salary > 700000 && salary <= 1000000)
            //{
            //    decimal totalTaxDeduction = 0;
            //    totalTaxDeduction += 500000 * 5 / 100;
            //    totalTaxDeduction += 200000 * 7 / 100;
            //    totalTaxDeduction += (salary - 700000) * 10 / 100;
            //    return totalTaxDeduction;
            //}
            //else
            //{
            //    decimal totalTaxDeduction = 0;
            //    totalTaxDeduction += 500000 * 5 / 100;
            //    totalTaxDeduction += 200000 * 7 / 100;
            //    totalTaxDeduction += 300000 * 10 / 100;
            //    totalTaxDeduction = (salary - 1000000) * 12 / 100;
            //    return totalTaxDeduction;
            //} 
            #endregion
        }

        private decimal GetTaxPercent(decimal salary)
        {
            switch (salary)
            {
                case <= 500000:
                    return 5;
                case > 500000 and <= 700000:
                    return 7;
                case > 700000 and <= 1000000:
                    return 10;
                default:
                    return 12;
            }
        }
    }
}
