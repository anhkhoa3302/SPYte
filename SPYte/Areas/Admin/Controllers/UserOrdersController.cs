using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPYte.Data;
using SPYte.Models;

namespace SPYte.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserOrders.Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/UserOrders/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.UserOrders == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrders
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            return View(userOrder);
        }

        // GET: Admin/UserOrders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/UserOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,GrandTotal,CustomerName,PhoneNumber,Email,AddressId,Status,Shipping,ShippingVia,UserId,CreatedDate,UpdatedDate")] UserOrder userOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOrder.UserId);
            return View(userOrder);
        }

        // GET: Admin/UserOrders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.UserOrders == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrders.FindAsync(id);
            if (userOrder == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOrder.UserId);
            return View(userOrder);
        }

        // POST: Admin/UserOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Total,GrandTotal,CustomerName,PhoneNumber,Email,AddressId,Status,Shipping,ShippingVia,UserId,CreatedDate,UpdatedDate")] UserOrder userOrder)
        {
            if (id != userOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOrderExists(userOrder.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOrder.UserId);
            return View(userOrder);
        }

        // GET: Admin/UserOrders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.UserOrders == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrders
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            return View(userOrder);
        }

        // POST: Admin/UserOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.UserOrders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserOrders'  is null.");
            }
            var userOrder = await _context.UserOrders.FindAsync(id);
            if (userOrder != null)
            {
                _context.UserOrders.Remove(userOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOrderExists(long id)
        {
          return (_context.UserOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
