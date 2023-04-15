using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SPYte.Data;
using SPYte.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SPYte.Areas.Identity.Pages.Account.Manage
{
    public class DetailUserProductsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DetailUserProductsModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _context = context;
        }

        public long Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public double Quantity { get; set; }

        public string Unit { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<ProductImg> Images { get; set; }
        public List<Category> productCategories { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        private async Task LoadAsync(long id)
        {
            var product = await _context.Products.FindAsync(id);
            Images = await _context.ProductImgs.Where(s => s.ProductId == id).ToListAsync();

            Id = id;
            Name = product.Name;
            Summary = product.Summary;
            Description = product.Description;
            Price = product.Price;
            Quantity = (double)product.Stock;
            Unit = product.Unit;
            if(product.CreatedDate != null)
                CreatedAt = (System.DateTime)product.CreatedDate;
            if (product.UpdatedDate != null)
                UpdatedAt = (System.DateTime)product.UpdatedDate;
            else
                UpdatedAt = CreatedAt;

            productCategories = await _context.ProductCategory.Include(x=>x.Category).Where(s=>s.ProductId==id).Select(cat=>cat.Category).ToListAsync();
           

        }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            Username = user.UserName;
            await LoadAsync(id);
            return Page();
        }

    }
}
