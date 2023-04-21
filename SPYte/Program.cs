using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPYte.Data;
using SPYte.Models;
using EmailService;
using VNPayment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Session for shopping cart
builder.Services.AddSession(cfg =>
{
    cfg.Cookie.Name = "ShoppingCart";
    cfg.IdleTimeout = new TimeSpan(24, 0, 0);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//EmailService
var emailConfig = builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailSender, EmailSender>();

//VnPayment
var VNPayConfig = builder.Configuration.GetSection("VNPayConfig").Get<VNPayConfig>();
builder.Services.AddSingleton(VNPayConfig);

builder.Services.AddScoped<IVNPayPayment, VNPayPayment>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Administrator}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapRazorPages();
});

app.Run();
