using System.Collections.Generic;
using System.Threading.Tasks;
using WedsiteBanHang.Models;

namespace WedsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        string? GetById(int id);
        string? GetAll();

        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}