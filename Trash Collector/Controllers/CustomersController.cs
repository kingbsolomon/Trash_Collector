using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Trash_Collector.ActionFilters;
using Trash_Collector.Data;
using Trash_Collector.Models;

namespace Trash_Collector.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Customers
        public ActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();

            if (customer == null)
            {
                return RedirectToAction("Create");
            }
            return View(customer);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id) //? handles the null case
        {
            var customer = _context.Customer.Where(c => c.Id == id).SingleOrDefault();

            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create([Bind("Id,IdentityUserId,FirstName,LastName,PhoneNumber,Address,City,State,ZipCode,CustomerBalance,DayWeek")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.IdentityUserId = userId;
                _context.Customer.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind("Id,IdentityUserId,FirstName,LastName,PhoneNumber,Address,City,State,ZipCode,DayWeek")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    customer.IdentityUserId = userId;
                    //_context.Entry(customer).State = EntityState.Modified;
                    _context.Customer.Update(customer);
                    _context.SaveChanges();
                }
                catch
                {
                    return View();
                }
                return RedirectToAction("Details");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();

            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            customer.IdentityUserId = userId;
            _context.Customer.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RequestPickup(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            customer.BeenPicked = false;
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult RequestPickup(int id, [Bind("CustomPickup,BeenPicked")] Customer customer)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var custInfo = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            custInfo.CustomPickup = customer.CustomPickup;
             _context.SaveChanges();
            return RedirectToAction("Details");
        }


        public ActionResult ChangeWeeklyPickup(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult ChangeWeeklyPickup(int id, [Bind("DayWeek")] Customer customer)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var custInfo = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            custInfo.DayWeek = customer.DayWeek;
            _context.SaveChanges();
            return RedirectToAction("Details");
        }
        public ActionResult TempSuspendPickup(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult TempSuspendPickup(int id, [Bind("SuspendStart,SuspendEnd")] Customer customer)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var custInfo = _context.Customer.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            custInfo.SuspendStart = customer.SuspendStart;
            custInfo.SuspendEnd = customer.SuspendEnd;
            _context.SaveChanges();
            return RedirectToAction("Details");
        }

    }
}
