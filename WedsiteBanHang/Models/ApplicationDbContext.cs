using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Thêm cái này
using Microsoft.EntityFrameworkCore;

namespace WedsiteBanHang.Models
{
    // Đổi DbContext thành IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}