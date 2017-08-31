﻿using System;
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
            return View(await _context.Payments.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.Include(x=>x.ApplicationUser).Include(x=>x.NonAccountUser)
                .SingleOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payment/Create
        public IActionResult Create()
        {
            ViewBag.OrderId = _context.Orders.LastOrDefault().OrderId;
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            if(form != null)
            {
                var model = new NonAccountUser();
                model.FirstName = form["FirstName"];
                model.LastName = form["LastName"];
                model.Street = form["Street"];
                model.ZipCode = Convert.ToInt32(form["ZipCode"]);
                model.Phone = form["Phone"];
                model.Email = form["Email"];
                model.City = form["City"];
                model.OrderId = Convert.ToInt32(form["OrderId"]);
                _context.NonAccountUsers.Add(model);
                _context.SaveChanges();
                var user = _context.NonAccountUsers.Last();
                ViewBag.NonAccountUserId = user.NonAccountUserId;
                ViewBag.OrderId = user.OrderId;
            };
           

            return View();
        }

        public IActionResult SavePayment(IFormCollection form)
        {

            var paymentChoize = Convert.ToInt32(form["creditCardRadio"]);

            var model = new Payment();

            if (String.IsNullOrEmpty(form["NonAccountUserId"]))
            {
                model.ApplicationuserId = form["UserId"];
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

                var order = _context.Orders.Include(i => i.Cart).ThenInclude(p => p.CartItems).FirstOrDefault(x => x.OrderId == model.OrderId);

                 if (order.ApplicationUserId != null)
                    {
                        var user = _userService.GetApplicationUserById(order.ApplicationUserId);
                        _emailservice.SendOrderConfirmToUser(order, user);
                        return RedirectToAction("ThankForOrdering");
                    }

            if (order.NonAccountUserId != 0)
                {
                var user = _userService.GetNonAccountUserById(order.NonAccountUserId);
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

        [Authorize(Roles = "admin")]
        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.Include(x => x.ApplicationUser).Include(x => x.NonAccountUser).SingleOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }
        [Authorize(Roles = "admin")]
        // POST: Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CardNumber,Cvv,Year,Month,IsPaid")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
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
