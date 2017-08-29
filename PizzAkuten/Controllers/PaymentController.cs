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

namespace PizzAkuten.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="admin")]
        // GET: Payment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Payment.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
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
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
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
            ViewBag.NonAccountUserId = user.Id;
            ViewBag.OrderId = user.OrderId;

            return View();
        }

        public IActionResult SavePayment(IFormCollection form)
        {
            var model = new Payment();
            model.NonAccountUserId = Convert.ToInt32(form["UserId"]);
            model.Month = Convert.ToInt32(form["Month"]);
            model.CardNumber = form["CardNumber"];
            model.Cvv = Convert.ToInt32(form["Cvv"]);
            model.Year = Convert.ToInt32(form["Year"]);
            model.OrderId = Convert.ToInt32(form["OrderId"]);
            _context.Payment.Add(model);
            _context.SaveChanges();

            return RedirectToAction("ThankForOrdering");
        }

        public IActionResult ThankForOrdering()
        {
            ViewBag.ThankYou = "Tack för Din Order!";

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

            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.PaymentId == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CardNumber,Cvv,Year,Month")] Payment payment)
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

            var payment = await _context.Payment
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
            var payment = await _context.Payment.SingleOrDefaultAsync(m => m.PaymentId == id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.PaymentId == id);
        }
    }
}
