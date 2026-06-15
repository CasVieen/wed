using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WedsiteBanHang.Models;

namespace WedsiteBanHang.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Hàm khởi tạo nhận DbContext (Ảnh 1 & Ảnh 2)
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả sản phẩm kèm danh mục (Ảnh 2)
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category) // Include thông tin về category
                .ToListAsync();
        }

        // Lấy sản phẩm theo ID kèm danh mục (Ảnh 3)
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm mới sản phẩm (Ảnh 3)
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Cập nhật sản phẩm (Ảnh 3)
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm theo ID (Ảnh 3)
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public string? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string? GetAll()
        {
            throw new NotImplementedException();
        }

        public void Add(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}