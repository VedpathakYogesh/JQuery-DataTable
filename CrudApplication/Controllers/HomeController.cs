using CrudApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudApplication.Controllers
{
    public class HomeController : Controller
    {
        NorthwindEntities Db = new NorthwindEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (NorthwindEntities Db = new NorthwindEntities())
            {
                var employee = Db.Employees.OrderBy(a=>a.FirstName).ToList();
                return Json(new { data = employee }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpGet]
        public ActionResult Save()
        {  
           return View();
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (NorthwindEntities Db = new NorthwindEntities())
                {
                    if (emp.EmployeeID > 0)
                    {
                        //Edit 
                        var v = Db.Employees.Where(a => a.EmployeeID == emp.EmployeeID).FirstOrDefault();
                        if (v != null)
                        {
                            v.FirstName = emp.FirstName;
                            v.LastName = emp.LastName;
                            v.EmailID = emp.EmailID;
                            v.City = emp.City;
                            v.Country = emp.Country;
                        }
                    }
                    else
                    {
                        //Save
                        Db.Employees.Add(emp);
                    }
                    Db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (NorthwindEntities dc = new NorthwindEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (NorthwindEntities dc = new NorthwindEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}