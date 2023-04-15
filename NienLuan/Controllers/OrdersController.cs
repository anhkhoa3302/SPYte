using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SPYte.Data;
using SPYte.Models;
using VNPayment;

namespace SPYte.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IVNPayPayment _vnpay;

        public OrdersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IVNPayPayment vnpay
            )
        {
            _context = context;
            _userManager = userManager;
            _vnpay = vnpay;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {

            ViewBag.Categories = await _context.Categories.ToListAsync();


            List<CartItem> cart = GetCartItems();

            if (cart.Count == 0) return Redirect("/Products");
            ViewBag.CartItem = cart;



            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var AddressList = await _context.Addresses.Where(p => p.UserId == user.Id).ToListAsync();
            if(AddressList.Count != 0)
                ViewBag.UserAddressList = new SelectList(AddressList, "Id", "Name");


            UserOrder order = new UserOrder();


            order.CustomerName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            order.Email = user.Email;
            order.PhoneNumber = user.PhoneNumber;
            order.UserId = user.Id;



            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,GrandTotal,CustomerName,PhoneNumber,Email,AddressId,Status,Shipping,ShippingVia,UserId,CreatedDate,UpdatedDate")] UserOrder order, string password)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var verigyResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (verigyResult == PasswordVerificationResult.Success) // either verigyResult equal to SuccessRehashNeeded or Success
            {
                if (ModelState.IsValid)
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    List<CartItem> listItem = GetCartItems();
                    foreach (var item in listItem)
                    {
                        OrderDetail orderItem = new OrderDetail();
                        orderItem.OrderId = order.Id;
                        orderItem.ProductId = item.Product.Id;
                        orderItem.Quantity = item.Quantity;
                        orderItem.TotalPrice = item.Quantity * item.Product.Price;
                        _context.Add(orderItem);
                    }
                    await _context.SaveChangesAsync();
                    ClearCart();
                    return Redirect("/Identity/Account/Manage/UserOrders");
                }
            }

            return RedirectToAction(nameof(Index));
        }







        private bool UserOrderExists(long id)
        {
          return (_context.UserOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        public List<CartItem> GetCartItems()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }
    }
}
