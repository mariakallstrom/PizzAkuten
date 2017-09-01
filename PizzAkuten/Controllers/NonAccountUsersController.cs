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
    public class NonAccountUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NonAccountUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NonAccountUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.NonAccountUsers.ToListAsync());
        }

        // GET: NonAccountUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nonAccountUser = await _context.NonAccountUsers
                .SingleOrDefaultAsync(m => m.NonAccountUserId == id);
            if (nonAccountUser == null)
            {
                return NotFound();
            }

            return View(nonAccountUser);
        }

        // GET: NonAccountUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NonAccountUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NonAccountUserId,FirstName,LastName,Street,ZipCode,City,Email,Phone,OrderId")] NonAccountUser nonAccountUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nonAccountUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nonAccountUser);
        }

        // GET: NonAccountUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nonAccountUser = await _context.NonAccountUsers.SingleOrDefaultAsync(m => m.NonAccountUserId == id);
            if (nonAccountUser == null)
            {
                return NotFound();
            }
            return View(nonAccountUser);
        }

        // POST: NonAccountUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NonAccountUserId,FirstName,LastName,Street,ZipCode,City,Email,Phone,OrderId")] NonAccountUser nonAccountUser)
        {
            if (id != nonAccountUser.NonAccountUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nonAccountUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NonAccountUserExists(nonAccountUser.NonAccountUserId))
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
            return View(nonAccountUser);
        }

        // GET: NonAccountUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nonAccountUser = await _context.NonAccountUsers
                .SingleOrDefaultAsync(m => m.NonAccountUserId == id);
            if (nonAccountUser == null)
            {
                return NotFound();
            }

            return View(nonAccountUser);
        }

        // POST: NonAccountUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nonAccountUser = await _context.NonAccountUsers.SingleOrDefaultAsync(m => m.NonAccountUserId == id);
            _context.NonAccountUsers.Remove(nonAccountUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NonAccountUserExists(int id)
        {
            return _context.NonAccountUsers.Any(e => e.NonAccountUserId == id);
        }
    }
}
