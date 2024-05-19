using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WorkZen.Models;
using WorkZen.CustomFilters.AuthenticationFilters;
using System.Web.Security;

namespace WorkZen.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        [HttpGet]
        [RedirectAuthEmpFilter]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [RedirectAuthEmpFilter]
        public ActionResult Login(string email, string password)
        {
            using(var _dbContext = new MitRaoEntities())
            {
                var MD5ForEnteredPassword = GetMD5(password);
                var check = _dbContext.Employees.FirstOrDefault(emp => emp.email == email && emp.password == MD5ForEnteredPassword);

                if(check != null)
                {
                    Session["email"] = email;
                    FormsAuthentication.SetAuthCookie(email, false);
                    //return Redirect("/Home/Index");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "**Email or Password is incorrect.";
                    ViewBag.email = email;
                    ViewBag.password = password;
                    return View();
                }
            }
        }

     
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("~/Auth/Login");
        }

        [HttpGet]
        [RedirectAuthEmpFilter]
        public ActionResult Register()
        {
            return View();  
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [RedirectAuthEmpFilter]
        public ActionResult Register(Employee _emp)
        {
            using(MitRaoEntities _dbContext = new MitRaoEntities())
            {
                var check = _dbContext.Employees.FirstOrDefault(emp => emp.email == _emp.email);
                if (check == null)
                {
                    try
                    {
                        _emp.password = GetMD5(_emp.password);
                        _emp.departmentId = 1;
                        if (string.IsNullOrEmpty(_emp.employeeCode))
                        {
                            // Set a default value, you can modify this as per your requirement
                            _emp.employeeCode = "WorkZen";
                        }
                        _dbContext.Employees.Add(_emp);
                        _dbContext.SaveChanges();
                        _emp.employeeCode = "WorkZen" + _emp.id;
                        _dbContext.SaveChanges();
                        Session["email"] = _emp.email;
                        return Redirect("~/Home/index");
                    } 
                    catch(DbEntityValidationException dbEx){
                        foreach(var validationerror in dbEx.EntityValidationErrors)
                        {
                            foreach (var error in validationerror.ValidationErrors)
                            {
                                Console.WriteLine(error);
                            }
                        }
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Entered email already exixts, LOL!!";
                    return View(_emp);
                }
            }
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}