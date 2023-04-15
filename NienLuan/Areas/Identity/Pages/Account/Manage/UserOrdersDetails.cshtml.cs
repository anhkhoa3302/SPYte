using System;
using System.Collections.Generic;
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
    public class UserOrdersDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserOrdersDetailsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public string Username { get; set; }
        public string UserId { get; set; }
        public UserOrder Order { get; set; }
        public List<OrderDetail> OrderItems { get; set; }

        private async Task LoadAsync(long id)
        {
            var order = await _context.UserOrders.FindAsync(id);
            var orderItems = await _context.OrderDetails.Include(n=>n.Product).ThenInclude(n=>n.ProductImgs).Where(p => p.OrderId == order.Id).ToListAsync();

            Order = order;
            OrderItems = orderItems;
        }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Username = user.UserName;
            UserId = user.Id;
            await LoadAsync(id);

            return Page();
        }
    }
}
