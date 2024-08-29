using Microsoft.EntityFrameworkCore;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineAuctionDbContext _context;

        public ProductRepository(OnlineAuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(string productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            // Ensure the product is being tracked
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found");
            }

            // Attach and update the entity
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

       
    }
}
