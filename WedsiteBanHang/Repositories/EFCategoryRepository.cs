using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WedsiteBanHang.Models;

namespace WedsiteBanHang.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        // Hàm khởi tạo nhận DbContext để tương tác với CSDL (Ảnh 3)
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách tất cả danh mục từ Database
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // 2. Lấy chi tiết một danh mục theo ID
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // 3. Thêm mới một danh mục
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        // 4. Cập nhật thông tin danh mục
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        // 5. Xóa danh mục theo ID
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            throw new NotImplementedException();
        }
    }
}