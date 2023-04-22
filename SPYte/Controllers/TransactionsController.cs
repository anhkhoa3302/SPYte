using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SPYte.Data;
using SPYte.Models;
using VNPayment;
using EmailService;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;

namespace SPYte.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IVNPayPayment _vnpay;
        private readonly EmailService.IEmailSender _emailSender;

        public TransactionsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IVNPayPayment vnpay,
            EmailService.IEmailSender emailSender
            )
        {
            _context = context;
            _userManager = userManager;
            _vnpay = vnpay;
            _emailSender = emailSender;
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
                        //UserOrder order = await _context.UserOrders.FindAsync(transaction.OrderId);
                        UserOrder order = await _context.UserOrders.Include(p => p.OrderDetails).Where(p => p.Id == transaction.OrderId).FirstOrDefaultAsync();
                        order.Status = 1;
                        order.UpdatedDate = DateTime.Now;
                        _context.UserOrders.Update(order);
                        await _context.SaveChangesAsync();

                        String message = "<p>Xin chào " + order.CustomerName + ",</p>" +
                            "<p><b>Chi tiết đơn hàng</b> :</p>" +
                            "<p><b>Ngày thanh toán</b> : " + order.UpdatedDate + "</p>" +
                            "<p><b>Địa chỉ giao</b> : " + order.AddressId + "</p>" +
                            "<p><b>Ngân hàng</b> : " + transaction.bankCode + "</p>" +
                            "<p><b>Mã giao dịch</b> : " + transaction.vnpayTranId + "</p>" +
                            "<p><b>Các sản phẩm đã mua</b> :</p>";

                        foreach (var item in order.OrderDetails)
                        {
                            var product = await _context.Products.FindAsync(item.ProductId);
                            if(product != null)
                            {
                                message += "<p>- " + product.Name  +" x"+item.Quantity +" : " + item.TotalPrice.ToString("#,###.#") + "vnđ</p>";
                            }
                        }
                        message += "<p><b>Shipping</b> : 15.000vnđ</p>";
                        message += "<p><b style = \"color:red\">Tổng tiền</b> : " + order.GrandTotal.ToString("#,###.#") + "vnđ</p>";
                            //"<b></b> : " + +"\n" +
                            //"<b></b> : " + +"\n" +



                        Message mssg = new Message(new string[] { user.Email },"Chi tiết đơn hàng",message);
                        _emailSender.SendEmail(mssg);
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
