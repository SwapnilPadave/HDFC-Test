using MachinTestForHDFC.Database;
using MachinTestForHDFC.Dtos;
using MachinTestForHDFC.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace MachinTestForHDFC.Services
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

        public async Task<Employees?> GetByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Employees> CreateEmployeeAsync(CreateEmployeeRequestDto request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = new Employees
                {
                    FullName = request.FullName,
                    EmpCode = request.EmpCode,
                    DepartmentId = request.DepartmentId,
                    Salary = request.Salary,
                    DOB = request.DOB,
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };
                var data = await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                var employeeTaxCalculation = new EmployeeTaxCalculationDetails
                {
                    EmployeeId = data.Entity.Id,
                    BasicSalary = request.Salary,
                    TaxDeductedSalary = CalculateTax(request.Salary),
                    TaxPercent = GetTaxPercent(request.Salary),
                    CreatedBy = 1,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };
                await _context.EmployeeTaxCalculationDetails.AddAsync(employeeTaxCalculation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return employee;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating employee: " + ex.Message);
            }
        }

        public async Task<Employees> UpdateEmployeeAsync(int id, UpdateEmployeeRequestDto request)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
                if (employee == null)
                {
                    throw new Exception("Employee not found");
                }
                var employeeTaxDetails = await _context.EmployeeTaxCalculationDetails
                    .FirstOrDefaultAsync(x => x.EmployeeId == id);

                employee.FullName = request.FullName;
                employee.EmpCode = request.EmpCode;
                employee.DepartmentId = request.DepartmentId;
                employee.Salary = request.Salary;
                employee.DOB = request.DOB;
                employee.ModifiedBy = 1;
                employee.ModifiedDate = DateTime.UtcNow;
                _context.Employees.Update(employee);

                if (employeeTaxDetails != null)
                {
                    employeeTaxDetails.BasicSalary = request.Salary;
                    employeeTaxDetails.TaxDeductedSalary = CalculateTax(request.Salary);
                    employeeTaxDetails.TaxPercent = GetTaxPercent(request.Salary);
                    employeeTaxDetails.ModifiedBy = 1;
                    employeeTaxDetails.ModifiedDate = DateTime.UtcNow;
                    _context.EmployeeTaxCalculationDetails.Update(employeeTaxDetails);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return employee;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error while updating employee: " + ex.Message);
            }
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
