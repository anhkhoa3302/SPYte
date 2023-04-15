using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPYte.Data;
using SPYte.Models;

namespace SPYte.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Addresses.Include(a => a.User).Include(a => a.WardCodeNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .Include(a => a.WardCodeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            var province = _context.Provinces;
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewBag.Provinces = new SelectList(province.ToList(), "Code", "Name");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddressDetail,WardCode,UserId")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", address.UserId);
            ViewBag.Provinces = new SelectList(_context.Provinces.ToList(), "Code", "Name");
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", address.UserId);
            ViewBag.Provinces = new SelectList(_context.Provinces.ToList(), "Code", "Name");
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,AddressDetail,CreatedDate,UpdatedDate,UserId,WardCode")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", address.UserId);
            ViewBag.Provinces = new SelectList(_context.Provinces.ToList(), "Code", "Name");
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .Include(a => a.WardCodeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Addresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Addresses'  is null.");
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }
            
            await _context.SaveChangesAsync();
            return Redirect("/Identity/Account/Manage/UserAddress");
        }

        private bool AddressExists(long id)
        {
          return (_context.Addresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<JsonResult> GetDistrict(string ProvinceId)
        {
            var districts = await _context.Districts.Where(z => z.ProvinceCode == ProvinceId).ToListAsync();

            return Json(new SelectList(districts, "Code", "Name"));
        }

        [HttpPost]
        public async Task<JsonResult> GetWard(string DistrictId)
        {
            var wards = await _context.Wards.Where(z => z.DistrictCode == DistrictId).ToListAsync();

            return Json(new SelectList(wards, "Code", "Name"));
        }

        [HttpPost]
        public async Task<String> GetAddressDetail(string AddressId)
        {
            long id = long.Parse(AddressId);
            var address = await _context.Addresses.Include(p=>p.WardCodeNavigation).ThenInclude(p=>p.DistrictCodeNavigation).ThenInclude(p=>p.ProvinceCodeNavigation).Where(p=>p.Id == id).FirstOrDefaultAsync();
            if (address == null) return "Invalid";
            return address.AddressDetail + ", " +
                    address.WardCodeNavigation.FullName + ", " +
                    address.WardCodeNavigation.DistrictCodeNavigation.FullName + ", " +
                    address.WardCodeNavigation.DistrictCodeNavigation.ProvinceCodeNavigation.FullName;
        }
    }
}
