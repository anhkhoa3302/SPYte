using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPYte.Data;
using SPYte.Models;
using System.Data;

namespace SPYte.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userlist = _context.Users.ToList();
            return View(userlist);
        }

        // POST: Admin/Products/Delete/5
        [ ActionName("Ban")]
        public async Task<IActionResult> Ban(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Status = 2; // đặt isVisible là 2, để ký hiệu là không còn hữu dụng
            _context.Entry(user).State = EntityState.Modified;

            // Lưu lại thay đổi và chuyển hướng trang
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ActionName("UnBan")]
        public async Task<IActionResult> UnBan(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Status = 1; // đặt isVisible là 1, huỷ gửi
            _context.Entry(user).State = EntityState.Modified;

            // Lưu lại thay đổi và chuyển hướng trang
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
