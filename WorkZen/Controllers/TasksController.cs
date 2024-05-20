using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WorkZen.CustomFilters.AuthenticationFilters;
using WorkZen.Models;
using WorkZen.Models.CustomModels;

namespace WorkZen.Controllers
{
    [RedirectUnAuthUser]
    public class TasksController : Controller
    {

        private readonly MitRaoEntities _dbContext;
        public TasksController() { 
            this._dbContext = new MitRaoEntities();
        }


        //GET- Get Datatable data for employee tasks for diretor
        [HttpPost]
        public ActionResult GetEmployeeTasksData(JqueryDatatbleParams EmployeeTasksParams)
        {
            //Getting Tasks Where Department Is employee
            //List<Task> employeeTasks = _dbContext.Tasks.Where(task => task.Employee2.Department.name == "Employee").ToList();
            //List<Task> FinalResult = employeeTasks;

            //logic for filter with search
            SqlParameter offset = new SqlParameter("@offset", EmployeeTasksParams.start);
            SqlParameter size = new SqlParameter("@size", EmployeeTasksParams.length);

            List<WorkZen.Models.Task> EmployeesPagedTasks;

            var SearchValue = EmployeeTasksParams.search.value;
            

            int TotalEmployeeTasks = this._dbContext.Database.SqlQuery<int>("select count(*) from Task t join Employee e on e.id = t.employeeId where e.departmentId = 1").Single();

            if (!string.IsNullOrEmpty(SearchValue))
            {
                EmployeesPagedTasks = this._dbContext.Tasks.Where(task => task.taskDate.ToString().Contains(SearchValue) ||
                task.taskName.ToLower().Contains(SearchValue.ToLower()) ||
                task.taskDescription.ToLower().Contains(SearchValue.ToLower()) ||
                task.employeeId.ToString().Contains(SearchValue) ||
                task.createdOn.ToString().Contains(SearchValue)).ToList().Skip(EmployeeTasksParams.start).Take(EmployeeTasksParams.length).ToList();

                var TasksJson = JsonConvert.SerializeObject(new
                {
                    draw = EmployeeTasksParams.draw,
                    recordsTotal =  TotalEmployeeTasks,
                    recordsFiltered = EmployeesPagedTasks.Count(),
                    data = EmployeesPagedTasks
                }, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DateFormatString = "yyyy-mm-dd"
                });

                return Content(TasksJson, "application/json");
            }   
            else
            {
                if (EmployeeTasksParams.order == null)
                {
                    EmployeesPagedTasks = this._dbContext.Database.SqlQuery<WorkZen.Models.Task>("exec get_paged_all_employee_tasks @offset, @size", offset, size).ToList();
                    var json = JsonConvert.SerializeObject(new
                    {
                        draw = EmployeeTasksParams.draw,
                        recordsTotal = TotalEmployeeTasks,
                        recordsFiltered = TotalEmployeeTasks,
                        data = EmployeesPagedTasks
                    }, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DateFormatString = "yyyy-MM-dd"
                    });

                    return Content(json, "application/json");
                }
                else
                {

                    EmployeesPagedTasks = this._dbContext.Database.SqlQuery<WorkZen.Models.Task>("exec get_paged_all_employee_tasks @offset, @size, @order_dir, @order_column", offset, size,
                        new SqlParameter("@order_dir", EmployeeTasksParams.order[0].dir),
                        new SqlParameter("@order_column", EmployeeTasksParams.columns[EmployeeTasksParams.order[0].column].data)).ToList();

                    var json = JsonConvert.SerializeObject(new
                    {
                        draw = EmployeeTasksParams.draw,
                        recordsTotal = TotalEmployeeTasks,
                        recordsFiltered = TotalEmployeeTasks,
                        data = EmployeesPagedTasks
                    }, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DateFormatString = "yyyy-MM-dd"
                    });

                    return Content(json, "application/json");
                }
            }
        }

        [Authorize(Roles = "Manager, Employee")]
        //GET - Create Task Home Page, This will listall tasks of employee as well
        [HttpGet]
        public ActionResult TaskHome()
        {
            var CurrentUser = HttpContext.Session["email"];
            var CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == CurrentUser);
            var Employees = _dbContext.Employees.ToList();
            var CurrentEmployeeTasks = _dbContext.Tasks
                                .Where(task => task.employeeId == CurrentEmployee.id)
                                .ToList();
            var Emp_EmpListTuple = (CurrentEmployee, Employees, CurrentEmployeeTasks);
            return View("~/Views/Home/CreateTask.cshtml", Emp_EmpListTuple);
        }

        [Authorize(Roles = "Manager, Employee")]
        //POST- create task post action to create task for employee
        [HttpPost]
        public ActionResult CreateTask(string taskName, string taskDescription, string approverId, string taskDate)
        {
            WorkZen.Models.Task task = new WorkZen.Models.Task();
            var ApproverOfTask = _dbContext.Employees.FirstOrDefault(emp => emp.employeeCode == approverId);
            var CreatorEmail = HttpContext.Session["email"];
            var CreatorOfTask = _dbContext.Employees.FirstOrDefault(emp => emp.email == CreatorEmail);
            task.taskName = taskName;
            task.taskDescription = taskDescription;
            task.taskDate = DateTime.Now;
            task.approverId = ApproverOfTask.id;
            task.status = "pending";
            task.employeeId = CreatorOfTask.id;
            task.createdOn = DateTime.Now;
            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();
            task.status = task.status.Trim();
            _dbContext.SaveChanges();
            return new RedirectResult("/");
        }

        [Authorize(Roles = "Manager, Employee")]
        //GET- get a partial view pop up to create a task log entry
        [HttpGet]
        public PartialViewResult GetTaskPopUp()
        {
            var empEmail = HttpContext.Session["email"];
            var CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == empEmail);
            var Employees = _dbContext.Employees.ToList();
            WorkZen.Models.Task TaskToEdit = null;
            var Emp_EmpListTuple = (CurrentEmployee, TaskToEdit);
            return PartialView("_TaskPopUpPartial", Emp_EmpListTuple);
        }

        [Authorize(Roles = "Manager, Employee")]
        //GET- Get Edit Task PopUp 
        [HttpGet]
        public PartialViewResult GetModifyTaskPopUp(int id)
        {
            var empEmail = HttpContext.Session["email"];
            var CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == empEmail);
            WorkZen.Models.Task TaskToEdit = _dbContext.Tasks.FirstOrDefault(task => task.id == id);
            var Emp_EmpListTuple = (CurrentEmployee, TaskToEdit);
            return PartialView("_TaskPopUpPartial", Emp_EmpListTuple);
        }

        [Authorize(Roles = "Manager, Employee")]
        //POST-Modify/ Update Existing Task
        [HttpPost]
        public ActionResult ModifyTask(string taskId, string taskName, string taskDescription, string taskDate)
        {
            var TaskId = Convert.ToInt64(taskId);
            var TaskToUpdate = _dbContext.Tasks.First(task => task.id == TaskId);
            TaskToUpdate.taskName = taskName;
            TaskToUpdate.taskDescription = taskDescription;
            if (taskDate != null)
            {
                TaskToUpdate.taskDate = Convert.ToDateTime(taskDate);
            }
            TaskToUpdate.modifiedOn = DateTime.Now;
            _dbContext.SaveChanges();
            return new RedirectResult("/");
        }

        [Authorize(Roles = "Manager, Director")]
        //POST - update task status - approve/reject
        [HttpPost]
        public ActionResult ApproveOrRejectTask(string TaskId, string TaskStatus)
        {
            var CurrentEmpEmail = HttpContext.Session["email"]; 
            Employee CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == CurrentEmpEmail);
            var TaskUpdateId = Convert.ToInt64(TaskId);
            var TaskToUpdate = _dbContext.Tasks.Where(task => task.id == TaskUpdateId).FirstOrDefault();
            TaskToUpdate.status = TaskStatus;
            TaskToUpdate.approvedOrRejectedBy = CurrentEmployee.id;
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Manager, Employee")]
        //GET - delete task
        [HttpGet]
        public PartialViewResult GetDeleteTaskPopUp(int id)
        {
            var TaskToDelete = _dbContext.Tasks.FirstOrDefault(task => task.id == id);
            return PartialView("_TaskDelPopUp", TaskToDelete);
        }

        [Authorize(Roles = "Manager, Employee")]
        //POST- Delete Task
        [HttpPost]
        public ActionResult DeleteTask(string taskId)
        {
            var IdOfTaskToDel = Convert.ToInt64(taskId);
            WorkZen.Models.Task TaskToDel = _dbContext.Tasks.FirstOrDefault(task => task.id == IdOfTaskToDel);
            _dbContext.Tasks.Remove(TaskToDel);
            _dbContext.SaveChanges();
            return new RedirectResult("~/Tasks/TaskHome");
        }

        [Authorize(Roles = "Employee, Director")]
        //GET - Provide Team Tasks Of Manager 
        public PartialViewResult EmployeeTasksHome()
        {
            var CurrentEmployeeEmail = HttpContext.Session["email"];
            //Employee CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == CurrentEmployeeEmail);
            //List<Task> TeamTasks = _dbContext.Tasks.ToList();
            //if(CurrentEmployee.Department.name == "Director")
            //{
            //    TeamTasks = _dbContext.Tasks.Where(task => task.Employee2.Department.name == "Employee").ToList();
            //}
            //else
            //{
            //    TeamTasks = _dbContext.Tasks.Where(task => task.approverId == CurrentEmployee.id).ToList();
            //}
            
            //var Emp_Tasks = (CurrentEmployee, TeamTasks);   
            return PartialView("_EmployeeTasksHome");
        }

        [Authorize(Roles = "Manager, Director")]
        //Employee 2 will be the creator of task and employee 1 will be approver of task
        //GET - Provide Team Tasks Of Manager 
        public PartialViewResult ManagerTasksHome()
        {
            var CurrentEmployeeEmail = HttpContext.Session["email"];
            Employee CurrentEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.email == CurrentEmployeeEmail);
            List<WorkZen.Models.Task> TeamTasks = _dbContext.Tasks.ToList();
            if (CurrentEmployee.Department.name == "Director")
            {
                TeamTasks = _dbContext.Tasks.Where(task => task.Employee2.departmentId == 2).ToList();
            }
            else
            {
                TeamTasks = _dbContext.Tasks.Where(task => task.approverId == CurrentEmployee.id).ToList();
            }

            var Emp_Tasks = (CurrentEmployee, TeamTasks);
            return PartialView("_ManagerTasksHome", Emp_Tasks);
        }
    }
}