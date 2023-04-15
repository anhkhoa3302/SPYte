using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SPYte.Data;
using SPYte.Models;
using VNPayment;

namespace SPYte.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IVNPayPayment _vnpay;

        public TransactionsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IVNPayPayment vnpay
            )
        {
            _context = context;
            _userManager = userManager;
            _vnpay = vnpay;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(string OrderId, string Amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var order = await _context.UserOrders.Where(a => a.Id == long.Parse(OrderId)).SingleOrDefaultAsync();
            if(order.Status == 1) 
            {
                return Redirect("/Identity/Account/Manage/UserOrders");
            }


            var vnpayPaymentRequest = new VnPayRequest
            {
                OrderId = OrderId,
                Amount = Amount,
                OrderInfo = "Thông tin đơn hàng #" + OrderId,
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };
            var vnpayPaymentUrl = _vnpay.CreatePaymentUrl(vnpayPaymentRequest);
            return Redirect(vnpayPaymentUrl);
        }
        public async Task<IActionResult> PayReturn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Request.Query.Count > 0)
            {
                var vnpayData = Request.Query;
                SortedList<String, String> data = new SortedList<String, String>();

                foreach (KeyValuePair<string, StringValues> kv in vnpayData)
                {
                    data.Add(kv.Key, kv.Value);
                }
                _vnpay.AddResponse(data);

                Transaction transaction = new()
                {
                    OrderId = long.Parse(_vnpay.GetResponseData("vnp_TxnRef").ToString()),
                    UserId = user.Id,
                    vnpayTranId = Convert.ToInt64(_vnpay.GetResponseData("vnp_TransactionNo")),
                    vnp_ResponseCode = _vnpay.GetResponseData("vnp_ResponseCode"),
                    vnp_TransactionStatus = _vnpay.GetResponseData("vnp_TransactionStatus"),
                    vnp_SecureHash = Request.Query["vnp_SecureHash"].ToString(),
                    TerminalID = Request.Query["vnp_TmnCode"].ToString(),
                    vnp_Amount = Convert.ToDouble(_vnpay.GetResponseData("vnp_Amount")) / 100,
                    Unit = "VND",
                    bankCode = Request.Query["vnp_BankCode"].ToString()
                };

                bool checkSignature = _vnpay.ValidateSignature(transaction.vnp_SecureHash);

                if (checkSignature)
                {
                    if (transaction.vnp_ResponseCode == "00" && transaction.vnp_TransactionStatus == "00")
                    {
                        transaction.Status = true;
                        UserOrder order = await _context.UserOrders.FindAsync(transaction.OrderId);
                        order.Status = 1;
                        order.UpdatedDate = DateTime.Now;
                        _context.UserOrders.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        transaction.Status = false;
                    }
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();


                }

                return View(transaction);

            }

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public async Task<IActionResult> Details(long id)
        {
            var transaction = await _context.Transactions.Where(p => p.OrderId == id).SingleOrDefaultAsync();
            return View(transaction);
        }
    }
}
