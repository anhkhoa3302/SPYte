using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPYte.Data;
using SPYte.Models;

namespace SPYte.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(long? category, string text, int pg = 1)
        {
            List<Category> categories = await _context.Categories.Include(p => p.ProductCategory).ToListAsync();

            ViewBag.PopCat = categories.OrderByDescending(p => p.ProductCategory.Count()).Take(4).ToList();

            ViewBag.Categories = await _context.Categories.ToListAsync();

            List<Product> shshopContext;
            if (category == null)
            {
                if (text == null)
                {
                    shshopContext = await _context.Products.Include(p => p.User).Include(n => n.ProductImgs).Where(n => n.Status == 1).OrderBy(p => p.Name).ToListAsync();
                    ViewBag.ProductListTitle = "Tất cả sản phẩm";
                }
                else
                {
                    shshopContext = await _context.Products.Include(p => p.User).Include(n => n.ProductImgs).Where(n => n.Status == 1 && n.Name.Contains(text.ToLower())).OrderBy(p => p.Name).ToListAsync();
                    ViewBag.ProductListTitle = "Tìm kiếm";
                }

            }
            else
            {
                shshopContext = await _context.Products.Include(p => p.User).Include(n => n.ProductImgs).Include(n => n.ProductCategory).Where(n => n.Status == 1 && n.ProductCategory.Any(x => x.CategoryId == category)).OrderBy(p => p.Name).ToListAsync();
                Category cat = await _context.Categories.FindAsync(category);
                ViewBag.ProductListTitle = cat.Name;
                ViewBag.CatBasedList = category;
            }



            var NewestProduct = await _context.Products.Include(p => p.User).Include(n => n.ProductImgs).Where(n => n.Status == 1).ToListAsync();
            ViewBag.NewestProduct = NewestProduct.OrderByDescending(x => x.CreatedDate).Take(3).ToList();

            const int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = shshopContext.Count();

            ViewBag.ProSum = recsCount;

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = shshopContext.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(data);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.User)
                .Include(n => n.ProductImgs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.ProductCategory.Include(x => x.Category).Where(s => s.ProductId == id).Select(cat => cat.Category).ToListAsync();

            return View(product);
        }

        // GET: Products/Create
        //public IActionResult Create()
        //{
        //    ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
        //    return View();
        //}

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserId,Title,Summary,Content,Price,Quantity,Unit,Status,CreatedAt,UpdatedAt")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", product.UserId);
        //    return View(product);
        //}

        // GET: Products/Edit/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    var product = await _context.Product.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    if (product.UserId != user.Id)
        //    {
        //        return NotFound($"You can't alter this product");
        //    }
        //    ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", product.UserId);
        //    return View(product);
        //}

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,Title,Summary,Content,Price,Quantity,Unit,Status,CreatedAt,UpdatedAt")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return Redirect("/Identity/Account/Manage/UserProducts");
        //    }
        //    else
        //    {
        //        return NotFound("Model state invalid");
        //    }
        //}

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

    }
}
