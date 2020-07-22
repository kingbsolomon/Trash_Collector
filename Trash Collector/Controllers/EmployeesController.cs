using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Trash_Collector.ActionFilters;
using Trash_Collector.Data;
using Trash_Collector.Models;

namespace Trash_Collector.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DateTime currentDay = DateTime.Now;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public ActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();

            if (employee == null)
            {
                return RedirectToAction("Create");
            }
            return RedirectToAction("Default");
        }

        public ActionResult FilterByDay(string DayofWeek = "")
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();
            var customers = _context.Customer.Where(c =>c.ZipCode == employee.ZipCode).ToList();

            if (!string.IsNullOrEmpty(DayofWeek))
            {
                customers = _context.Customer.Where(c => c.DayWeek == DayofWeek).ToList();
            }
            return View(customers);
        }

        public ActionResult Default()
        {
            DefaultEmployeeViewModel defaultEmployee = new DefaultEmployeeViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();
            var dateCheck = _context.Customer.Where(d => d.CustomPickup != default).ToList();
            defaultEmployee.Customers = _context.Customer.Where(c => c.ZipCode == employee.ZipCode && c.DayWeek == currentDay.DayOfWeek.ToString() && c.SuspendStart != currentDay && c.SuspendEnd < currentDay).ToList();
            foreach (Customer customer in dateCheck)
            {
                if (customer.CustomPickup.DayOfWeek.ToString() == currentDay.DayOfWeek.ToString())
                {
                    defaultEmployee.Customers.Add(customer);
                }
            }
            if (defaultEmployee.Customers == null)
            {
                return Content("<script>alert('None Found');</script>");
            }
            defaultEmployee.DaysOfWeekList = new SelectList(new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
            return View(defaultEmployee);
        }

        [HttpPost]
        public ActionResult Default(DefaultEmployeeViewModel defaultEmployee)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();
            var dateCheck = _context.Customer.Where(d => d.CustomPickup != default).ToList();

            if (defaultEmployee.SelectedDay is null)
            {
                defaultEmployee.Customers = _context.Customer.Where(c => c.ZipCode == employee.ZipCode && (c.DayWeek == currentDay.DayOfWeek.ToString() || c.CustomPickup.ToString("dddd") == currentDay.ToString("dddd")) && c.SuspendStart != currentDay && c.SuspendEnd < currentDay).ToList();
            }
            else
            {
                defaultEmployee.Customers = _context.Customer.Where(c => c.ZipCode == employee.ZipCode && c.DayWeek == defaultEmployee.SelectedDay && c.SuspendStart != currentDay && c.SuspendEnd < currentDay).ToList();
            }
            foreach (Customer customer in dateCheck)
            {
                if (customer.CustomPickup.DayOfWeek.ToString() == defaultEmployee.SelectedDay)
                {
                    defaultEmployee.Customers.Add(customer);
                }
            }
            if (defaultEmployee.Customers == null)
            {
                return Content("<script>alert('None Found');</script>");
            }
            defaultEmployee.DaysOfWeekList = new SelectList(new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
            return View(defaultEmployee);
        }
        public void CheckPickStatus(int id)
        {
            var customer = _context.Customer.Where(c => c.Id == id).SingleOrDefault();

            if (customer.CustomPickup.ToString("dddd") == currentDay.ToString("dddd") || customer.DayWeek == customer.CustomPickup.DayOfWeek.ToString())
            {
                customer.BeenPicked = false;
                _context.Update(customer);
            }
        }
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default");
            }
            var customer = _context.Customer.Where(c => c.Id == id).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Default");
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult Approve(int id, [Bind("BeenPicked")] Customer customer)
        {
            CheckPickStatus(id);
            var custUpdate = _context.Customer.Where(c => c.Id == id).SingleOrDefault();

            if (!custUpdate.BeenPicked)
            {
                if (custUpdate.CustomPickup.ToString("dddd") == currentDay.ToString("dddd"))
                {
                    custUpdate.CustomPickup = default;
                    custUpdate.CustomerBalance += 25;
                    custUpdate.BeenPicked = true;
                    _context.SaveChanges();
                }
                else if (custUpdate.DayWeek == currentDay.DayOfWeek.ToString())
                {
                    custUpdate.CustomerBalance += 25;
                    custUpdate.BeenPicked = true;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Default");
        }

        // GET: Employees/Details/5
        public ActionResult DetailsCustomer(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default");
            }

            var customer = _context.Customer.Where(c=> c.Id == id).SingleOrDefault();
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public ActionResult DetailsEmployee(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Default");
            }

            var employee = _context.Employee.Where(c => c.Id == id).SingleOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind("FirstName,LastName,EmailAddress,ZipCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                employee.IdentityUserId = userId;
                _context.Employee.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _context.Employee.Where(e => e.Id == id).SingleOrDefault();

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("FirstName,LastName,EmailAddress,ZipCode")] Employee employee)
        {
            if (id != employee.Id)
            {
                return RedirectToAction("Default");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    _context.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Default");
                }
                return RedirectToAction("Details");
            }
            return RedirectToAction("Default");
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _context.Employee
                .Include(e => e.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var employee = _context.Employee.Find(id);
            _context.Employee.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Default");
        }
    }
}
