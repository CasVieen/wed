using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;
using WedsiteBanHang.Models; // Nơi chứa SD và ApplicationUser
using WedsiteBanHang.Repositories;
// Chỉ định rõ ApplicationUser dùng từ Models để tránh xung đột với Data
using ApplicationUser = WedsiteBanHang.Models.ApplicationUser;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// 1. KẾT NỐI DATABASE
// =====================================================
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Không tìm thấy Connection String 'DefaultConnection' trong appsettings.json.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// =====================================================
// 2. CẤU HÌNH IDENTITY VÀ ROLE
// =====================================================
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Cấu hình mật khẩu (Cho phép mật khẩu dạng admin1@ hoặc Admin123@)
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredUniqueChars = 1;

        // Email không được trùng
        options.User.RequireUniqueEmail = true;

        // Chưa yêu cầu xác nhận email
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;

        // Khóa tài khoản nếu đăng nhập sai nhiều lần
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cấu hình Cookie đăng nhập
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// =====================================================
// 3. ĐĂNG KÝ DỊCH VỤ
// =====================================================
builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// =====================================================
// 4. TỰ ĐỘNG TẠO ROLE VÀ 3 TÀI KHOẢN ADMIN MẶC ĐỊNH
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 4.1. Tạo danh sách Roles
    string[] roleNames =
    {
        SD.Role_Admin,
        SD.Role_Employee,
        SD.Role_Company,
        SD.Role_Customer
    };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var createRoleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (!createRoleResult.Succeeded)
            {
                var errors = string.Join("; ", createRoleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Không thể tạo Role '{roleName}': {errors}");
            }
        }
    }

    // 4.2. Khởi tạo 3 Admin mặc định
    var defaultAdmins = new List<(string Username, string Email, string Password, string FullName)>
    {
        ("admin1", "admin1@gmail.com", "Admin123@", "Administrator 1"),
        ("admin2", "admin2@gmail.com", "Admin123@", "Administrator 2"),
        ("admin3", "admin3@gmail.com", "Admin123@", "Administrator 3")
    };

    foreach (var admin in defaultAdmins)
    {
        // Đã sửa 'userManager' bỏ dấu gạch dưới cho đúng biến
        var user = await userManager.FindByEmailAsync(admin.Email);
        if (user == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = admin.Email, // Identity nên dùng Email làm UserName để đăng nhập
                Email = admin.Email,
                FullName = admin.FullName,
                EmailConfirmed = true // Kích hoạt luôn để đăng nhập được ngay
            };

            // Tạo User với mật khẩu mới
            var createResult = await userManager.CreateAsync(newAdmin, admin.Password);

            if (createResult.Succeeded)
            {
                // Gán quyền Admin cho User
                await userManager.AddToRoleAsync(newAdmin, SD.Role_Admin);
            }
        }
    }
}

// =====================================================
// 5. HTTP REQUEST PIPELINE
// =====================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// =====================================================
// 6. ĐỊNH TUYẾN CÁC ROUTE
// =====================================================
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

// =====================================================
// 7. DỊCH VỤ EMAIL GIẢ LẬP
// =====================================================
public sealed class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine("===== EMAIL GIẢ LẬP =====");
        Console.WriteLine($"Người nhận: {email}");
        Console.WriteLine($"Tiêu đề: {subject}");
        Console.WriteLine($"Nội dung: {htmlMessage}");
        Console.WriteLine("=========================");

        return Task.CompletedTask;
    }
}