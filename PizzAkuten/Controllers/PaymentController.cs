using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PizzAkuten.Services;
using System.Globalization;

namespace PizzAkuten.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderservice;
        private readonly IEmailSender _emailservice;
        private readonly UserService _userService;

        public PaymentController(ApplicationDbContext context, OrderService orderservice, IEmailSender emailservice, UserService userService)
        {
            _context = context;
            _orderservice = orderservice;
            _emailservice = emailservice;
            _userService = userService;
        }

        [Authorize(Roles ="admin")]
        // GET: Payment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payments.Include(p => p.Order);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.SingleOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payment/Create
        public IActionResult Create(int id)
        {
            if (id != 0)
            {
                var order = _context.Orders.FirstOrDefault(x => x.OrderId == id);
                ViewBag.OrderId = order.OrderId;
                if (order.NonAccountUserId != 0)
                {
                    ViewBag.NonAccountUserId = order.NonAccountUser.NonAccountUserId;
                }
            }

            ViewData["Month"] = new SelectList(Enumerable.Range(1, 12));
            ViewData["Year"] = new SelectList(Enumerable.Range(DateTime.Now.Year, 2037));
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {

            var paymentChoize = Convert.ToInt32(form["creditCardRadio"]);

            var model = new Payment();

            if (String.IsNullOrEmpty(form["NonAccountUserId"]))
            {
                model.ApplicationUserId = form["UserId"];
            }
            else
            {
                model.NonAccountUserId = Convert.ToInt32(form["NonAccountUserId"]);
            }
            if (paymentChoize != 3)
            {
                model.Month = Convert.ToInt32(form["Month"]);
                model.CardNumber = form["CardNumber"];
                model.Cvv = Convert.ToInt32(form["Cvv"]);
                model.Year = Convert.ToInt32(form["Year"]);
            }

            if (paymentChoize == 1)
            {
                model.PayMethod = "Visa";
            }
            else if (paymentChoize == 2)
            {
                model.PayMethod = "MasterCard";
            }
            else
            {
                model.PayMethod = "Swish";
            }

            model.OrderId = Convert.ToInt32(form["OrderId"]);

            _context.Payments.Add(model);
            _context.SaveChanges();

            var order = _context.Orders.Include(i => i.Cart).ThenInclude(p => p.CartItems).ThenInclude(d => d.Dish).FirstOrDefault(x => x.OrderId == model.OrderId);

            if (order.ApplicationUserId != null)
            {
                var user = _userService.GetApplicationUserById(order.ApplicationUserId);
                _emailservice.SendOrderConfirmToUser(order, user);
                return RedirectToAction("ThankForOrdering");
            }

            if (model.NonAccountUserId != 0)
            {
                var user = _userService.GetNonAccountUserById(model.NonAccountUserId);
                _emailservice.SendOrderConfirmToUser(order, user);
                return RedirectToAction("ThankForOrdering");
            }

            return RedirectToAction("Create");
        }

        public IActionResult ThankForOrdering()
        {
            _orderservice.DeleteSession();
            ViewBag.ThankYou = "Tack för Din Order! En bekräftelse har nu skickats till din email";
     
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Payment payment)
        {
            var updated = _context.Payments.Find(id);
            updated.IsPaid = payment.IsPaid;

            _context.Update(updated);
            _context.SaveChanges();

          
            return RedirectToAction("Index");


        }

        [Authorize(Roles = "admin")]
        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.ApplicationUser)
                .Include(p => p.NonAccountUser)
                .Include(p => p.Order)
                .SingleOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
        [Authorize(Roles = "admin")]
        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var payment = await _context.Payments.SingleOrDefaultAsync(m => m.PaymentId == id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
