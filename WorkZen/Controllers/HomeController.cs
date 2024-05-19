using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;
using WorkZen.CustomFilters.AuthenticationFilters;
using WorkZen.Models;

namespace WorkZen.Controllers
{
    [RedirectUnAuthUser]
    public class HomeController : Controller
    {
        private readonly MitRaoEntities _dbContext;

        public HomeController()
        {
            this._dbContext = new MitRaoEntities();
        }


        [Authorize(Roles = "Director,Employee,Manager")]
        [HttpGet]
        public ActionResult Index()
        {
            var CurrentUser = HttpContext.Session["email"];
            var CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == CurrentUser);
            var Employees = _dbContext.Employees.ToList();
            var Tasks = _dbContext.Tasks.Where(task => task.approverId == CurrentEmployee.id).ToList();
            var Emp_EmpListTuple = (CurrentEmployee, Employees, Tasks);

            if(CurrentEmployee.Department.name == "Manager" || CurrentEmployee.Department.name == "Employee")
            {
                return new RedirectResult("~/Tasks/TaskHome");
            }
            else
            {
                return View(Emp_EmpListTuple);
            }
        }

        [Authorize(Roles = "Director")]
        //GET - Single Employee Manage PopUp
        [HttpPost]
        public PartialViewResult GetSingleEmployeeManagePopUp(string empCode)
        {
            var CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.employeeCode == empCode);
            var Employees = _dbContext.Employees.ToList();
            var Departments = _dbContext.Departments.ToList();
            var Emp_EmpListTuple = (CurrentEmployee, Employees, Departments);
            return PartialView("_EmployeePopUpPartial", Emp_EmpListTuple);
        }

        [Authorize(Roles = "Director")]
        //Get Managers Page
        [HttpGet]
        public ActionResult GetManagers()
        {
            var LoggeedInUser = HttpContext.Session["email"];
            var CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == LoggeedInUser);
            var Employees = _dbContext.Employees.ToList();
            var Tasks = _dbContext.Tasks.Where(task => task.approverId == CurrentEmployee.id).ToList();
            var Emp_Employees = (CurrentEmployee, Employees, Tasks);
            return View("Managers", Emp_Employees);
        }

        [Authorize(Roles = "Director")]
        //POST- Update Employee
        [HttpPost]
        public ActionResult UpdateEmployee(string employeeCode, string reportingPerson, string department)
        {
            var EmployeeToUpdate = _dbContext.Employees.FirstOrDefault(emp => emp.employeeCode == employeeCode);
            var reportPersonCode = reportingPerson.Split('-')[0].Trim();
            EmployeeToUpdate.Employee2 = _dbContext.Employees.FirstOrDefault(emp => emp.employeeCode == reportPersonCode);
            var tasks = _dbContext.Tasks.ToList();
            List<Task> TasksOfEmployeeToUpdate = _dbContext.Tasks.Where(task => task.Employee2.id == EmployeeToUpdate.id).ToList();

            foreach(Task task in TasksOfEmployeeToUpdate)
            {
                task.approverId = EmployeeToUpdate.Employee2.id;
            }
            EmployeeToUpdate.Department = _dbContext.Departments.FirstOrDefault(dep => dep.name == department);
            _dbContext.SaveChanges();
            //_dbContext.Database.ExecuteSqlCommand("exec sp_update_employee",
            //    new SqlParameter("@id", EmployeeToUpdate.id),
            //    new SqlParameter(""

            //    );
            return new RedirectResult("/");
        }

        [Authorize(Roles = "Director")]
        //POST - GetSingleEmployeeDeletePopUp
        public PartialViewResult GetSingleEmployeeDeletePopUp(string empCode)
        {
            var EmployeeToDel = _dbContext.Employees.FirstOrDefault(emp => emp.employeeCode == empCode);
            return PartialView("_EmployeeDelPopUpPartial", EmployeeToDel);
        }

        [Authorize(Roles = "Director")]
        //POST- Delete Employee
        [HttpPost]
        public ActionResult DeleteEmployee(string EmployeeCode) {
            var EmployeeToDelete = _dbContext.Employees.FirstOrDefault(emp => emp.employeeCode == EmployeeCode);
            if(EmployeeToDelete != null)
            {
                _dbContext.Employees.Remove(EmployeeToDelete);
                _dbContext.SaveChanges();
                return new RedirectResult("/");
            }
            else
            {
                return new HttpStatusCodeResult(501, "internal server error");
            }
        }
    }
}