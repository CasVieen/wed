using Microsoft.EntityFrameworkCore;
using WedsiteBanHang.Models;
using WedsiteBanHang.Repositories;
using Microsoft.AspNetCore.Identity; // Thêm thư viện này theo sách

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình kết nối SQL Server thông qua Connection String trong appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình dịch vụ Identity quản lý tài khoản theo yêu cầu của giáo trình
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

// 3. Đăng ký các Repository chạy với Database thật (Lược bỏ hoàn toàn MockRepository)
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

// 4. Kích hoạt tính năng xác thực tài khoản (Bắt buộc phải nằm TRƯỚC UseAuthorization)
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"); // Đổi mặc định chạy thẳng vào trang Product theo bài học

app.Run();