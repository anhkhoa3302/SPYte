using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SPYte.Data;
using SPYte.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPYte.Areas.Identity.Pages.Account.Manage
{
    public class AddUserProductsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _env;

        public AddUserProductsModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment env
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _env = env;
        }

        public string UserId { get; set; }

        

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            [Display(Name = "Title")]
            [StringLength(256)]
            public string Name { get; set; } = null!;
            [Required]
            [Display(Name = "Summary")]
            public string Summary { get; set; } = null!;

            [Required]
            [Display(Name = "Description")]
            public string Description { get; set; } = null!;
            [Display(Name = "Price")]
            public double Price { get; set; }

            [Display(Name = "Quantity")]
            public double Stock { get; set; }

            [Required]
            [Display(Name = "Unit")]
            [StringLength(20)]
            public string Unit { get; set; } = null!;

            public List<long> Categories { get; set; } = null!;

            public List<IFormFile>? Files { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = new SelectList(categories.ToList(), "Id", "Name"); 
            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync(IEnumerable<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            bool tagged = false;
            for (int n = 0; n < 5; n++)
            {
                if (Input.Categories[n] != -1)
                {
                    bool valid = false;
                    var t = await _context.Categories.FindAsync(Input.Categories[n]);
                    if(t!=null) valid = true;
                    if(!valid)
                    {
                        var categories = _context.Categories.ToList();
                        ViewData["Categories"] = new SelectList(categories.ToList(), "Id", "Name");
                        StatusMessage = "Error: Tag invalid";
                        return Page();
                    }
                    tagged = true;
                }
                
            }

            if(!tagged)
            {
                var categories = _context.Categories.ToList();
                ViewData["Categories"] = new SelectList(categories.ToList(), "Id", "Name");
                StatusMessage = "Error: Product need at least 1 tag";
                return Page();
            }


            if (ModelState.IsValid)
            {
                string[] pathList = new string[20];
                string[] filenameList = new string[20];
                string[] extList = { ".png",".jpeg",".jpg" };
                int i = 0;
                int fileCount = files.Count();
                if (  fileCount >= 6 && fileCount <= 20)
                {
                    foreach (var file in files)
                    {
                        if(file.Length > 5242880)
                        {
                            var categories = _context.Categories.ToList();
                            ViewData["Categories"] = new SelectList(categories.ToList(), "Id", "Name");
                            StatusMessage = "Error: file too big (only allow 5Mb or lower)";
                            return Page();
                        }
                        var imgName = Guid.NewGuid().ToString() + file.FileName;
                        var productimgsDir = Path.Combine(_env.WebRootPath, "files/products/images");
                        var path = Path.Combine(productimgsDir, imgName);
                        var ext = Path.GetExtension(path).ToLower();
                        bool valid = false;
                        foreach(var item in extList)
                        {
                            if (ext.Equals(item))
                                valid = true;

                        }
                        if(valid)
                        {
                            pathList[i] = path;
                            filenameList[i] = imgName;
                        }
                        else
                        {
                            var categories = _context.Categories.ToList();
                            ViewData["Categories"] = new SelectList(categories.ToList(), "Id", "Name");
                            StatusMessage = "Error: png, jpeg or jpg only";
                            return Page();
                        }
                        if (!Directory.Exists(productimgsDir))
                        {
                            Directory.CreateDirectory(productimgsDir);
                        }
                        i++;
                    }
                }
                else
                {
                    var categories = _context.Categories.ToList();
                    ViewData["Categories"] = new SelectList(categories.ToList(), "Id", "Name");
                    StatusMessage = "Please upload at least 6 and less than 20 images";
                    return Page();
                }
                Product product = new Product();
                product.UserId = await _userManager.GetUserIdAsync(user);
                product.Name = Input.Name;
                product.Summary = Input.Summary;
                product.Description = Input.Description;
                product.Price = Input.Price;
                product.Stock = Input.Stock;
                product.Unit = Input.Unit;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                int y = 0;
                foreach(var file in files)
                {
                    var path = pathList[y];
                    using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(stream);
                    }
                    ProductImg img = new ProductImg();
                    img.ProductId = product.Id;
                    img.Url = filenameList[y];
                    _context.ProductImgs.Add(img);
                    y++;
                }

                for (int n = 0; n < 5; n++)
                {
                    if (Input.Categories[n] == -1)
                    {
                        continue;
                    }
                    ProductCategory productCategory = new ProductCategory();
                    productCategory.ProductId = product.Id;
                    productCategory.CategoryId = Input.Categories[n];
                    _context.ProductCategory.Add(productCategory);
                }


                await _context.SaveChangesAsync();
                return RedirectToPage("./UserProducts");
            }
            StatusMessage = "Model state invalid";
            return RedirectToPage();
        }
    }
}
