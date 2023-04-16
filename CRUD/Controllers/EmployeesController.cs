using CRUD.Data;
using CRUD.Models;
using CRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext aPPDbContext;

        public EmployeesController(AppDbContext aPPDbContext)
        {
            this.aPPDbContext = aPPDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var employees= await aPPDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id=Guid.NewGuid(),
                Name=addEmployeeRequest.Name,
                DateOfBirth=addEmployeeRequest.DateOfBirth,
                Department=addEmployeeRequest.Department,
                Email=addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary
            };

            await aPPDbContext.Employees.AddAsync(employee);
            await aPPDbContext.SaveChangesAsync();
            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee=await  aPPDbContext.Employees.FirstOrDefaultAsync(X=> X.Id==id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                    Email = employee.Email,
                    Salary = employee.Salary
                };
                return await Task.Run(()=>View("View",viewModel));
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee= aPPDbContext.Employees.Find(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

               await aPPDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await aPPDbContext.Employees.FindAsync(model.Id);
            if(employee != null)
            {
                aPPDbContext.Employees.Remove(employee);
                await aPPDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
  
    }
}
