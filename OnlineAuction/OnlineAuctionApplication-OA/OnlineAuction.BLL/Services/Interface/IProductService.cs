using OnlineAuction.DAL.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.BLL.Services.Interface
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(string productId);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string productId);
        
    }
}
