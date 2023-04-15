using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPYte.Data;
using SPYte.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SPYte.Areas.Identity.Pages.Account.Manage
{
    public class UserAddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public UserAddressModel(
           UserManager<ApplicationUser> userManager,
           ApplicationDbContext context
           )
        {
            _userManager = userManager;
            _context = context;
        }



        [TempData]
        public string? StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public List<Address> AddressList { get; set; }



        public class InputModel
        {
            [Required]
            [Display(Name = "Address' Name")]
            public string name { get; set; } = null!;
            [Required]
            [Display(Name = "Road number, house number,...")]
            public string addressDetail { get; set; } = null!;
            public string? UserId { get; set; }
            public string? WardCode { get; set; }
        }


        private async Task LoadAsync(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var province = await _context.Provinces.ToListAsync();
            ViewData["Provinces"] = new SelectList(province.ToList(), "Code", "Name");


            AddressList = await _context.Addresses.Where(p => p.UserId == userId)
                .Include(p=>p.WardCodeNavigation).ThenInclude(p=>p.DistrictCodeNavigation).ThenInclude(p=>p.ProvinceCodeNavigation)
                .ToListAsync();

  
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            int countUserAddresses = await _context.Addresses.Where(p=>p.UserId == user.Id).CountAsync();
            if(countUserAddresses >= 3)
            {
                StatusMessage = "Each user can only have 3 address at a time";
                return RedirectToPage();
            }
            if (ModelState.IsValid)
            {
                var ward = await _context.Wards.FindAsync(Input.WardCode);
                Address address = new Address();
                address.WardCode = Input.WardCode;
                address.UserId = user.Id;
                address.AddressDetail = Input.addressDetail;
                address.Name = Input.name;

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();


                StatusMessage = "Address Update Successfully";
                return RedirectToPage("./UserAddress");
            }
            StatusMessage = "Model state invalid";
            return RedirectToPage();
        }
    }
}
