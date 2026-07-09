using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WedsiteBanHang.Models;
using WedsiteBanHang.Repositories;
using WedsiteBanHang.Data;

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
// 2. CẤU HÌNH IDENTITY VÀ ROLE (Sử dụng ApplicationUser từ Models)
// =====================================================
builder.Services
    // Đã sửa từ WedsiteBanHang.Data.ApplicationUser thành WedsiteBanHang.Models.ApplicationUser
    .AddIdentity<WedsiteBanHang.Models.ApplicationUser, IdentityRole>(options =>
    {
        // Cấu hình mật khẩu
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;

        // Email không được trùng
        options.User.RequireUniqueEmail = true;

        // Chưa yêu cầu xác nhận email vì đang dùng email giả lập
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
// Dịch vụ email giả lập
builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

// MVC và Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Repository
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// =====================================================
// 4. TỰ ĐỘNG TẠO CÁC ROLE (Chạy bất đồng bộ lúc khởi động)
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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

// Phải đặt Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

// =====================================================
// 6. ĐỊNH TUYẾN CÁC ROUTE (Bao gồm Area và Default)
// =====================================================
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

// Định tuyến cho các Identity Razor Pages (Login, Register, Logout...)
app.MapRazorPages();

// Chạy ứng dụng (Đặt lệnh này ở cuối cùng của file)
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