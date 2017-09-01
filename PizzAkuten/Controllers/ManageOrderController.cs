using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzAkuten.Data;
using PizzAkuten.Models;

namespace PizzAkuten.Controllers
{
    public class ManageOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ManageOrder
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        // GET: ManageOrder/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order =  _context.Orders.
               Include(x=>x.Payment).Include(x=>x.Cart).ThenInclude(p=>p.CartItems).ThenInclude(d=>d.Dish)
                .FirstOrDefault(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: ManageOrder/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["NonAccountUserId"] = new SelectList(_context.NonAccountUsers, "Id", "Id");
            return View();
        }

        // POST: ManageOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,TotalPrice,Delivered,ApplicationuserId,NonAccountUserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", order.ApplicationUserId);
            ViewData["NonAccountUserId"] = new SelectList(_context.NonAccountUsers, "Id", "Id", order.NonAccountUserId);
            return View(order);
        }

        // GET: ManageOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", order.ApplicationUserId);
            ViewData["NonAccountUserId"] = new SelectList(_context.NonAccountUsers, "Id", "Id", order.NonAccountUserId);
            return View(order);
        }

        // POST: ManageOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,TotalPrice,Delivered,ApplicationuserId,NonAccountUserId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", order.ApplicationUserId);
            ViewData["NonAccountUserId"] = new SelectList(_context.NonAccountUsers, "Id", "Id", order.NonAccountUserId);
            return View(order);
        }

        // GET: ManageOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.NonAccountUser)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: ManageOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
