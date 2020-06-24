using DemoDatatables.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DemoDatatables.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        NorthwindEntities _context = new NorthwindEntities();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Show Grid table
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowGrid()
        {
            return View();
        }

        /// <summary>
        /// Data onlad function
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadData()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();




                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var customerData = (from tempcustomer in _context.Customers
                                    select tempcustomer).ToList();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    // customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    // customerData = customerData.Where(m => m.CompanyName == searchValue).ToList();

                    // Contains Added fro search relevent data from list 

                    customerData = _context.Customers.Where(x => x.CompanyName.Contains(searchValue)).ToList();

                //    customerData = customerData.Where(m => m.CompanyName.StartsWith("searchValue")).ToList();
                }

                //total number of rows count     
                recordsTotal = customerData.Count();
                //Paging     
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            try
            {

                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var customerdata = _context.Customers.SingleOrDefault(e => e.CustomerID == ID);
                if (customerdata == null)
                {
                    return HttpNotFound();
                }
                return View(customerdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(Customer cs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(cs).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("ShowGrid");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            return View("ShowGrid");

        }

        //[HttpGet]
        //public ActionResult DeleteCustomer()
        //{
        //    return View();
        //}

        [HttpPost]
        public JsonResult DeleteCustomer(int? ID)
        {
            using (NorthwindEntities _context = new NorthwindEntities())
            {
                var customer = _context.Customers.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.Customers.Remove(customer);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }
    }
    
}