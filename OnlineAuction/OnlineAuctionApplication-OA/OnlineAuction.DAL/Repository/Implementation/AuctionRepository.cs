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
    public class AuctionRepository : IAuctionRepository
    {
        private readonly OnlineAuctionDbContext _context;

        public AuctionRepository(OnlineAuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Auction> GetByIdAsync(string auctionId)
        {
            return await _context.Auctions.FindAsync(auctionId);
        }

        public async Task AddAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string auctionId)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _context.Auctions.ToListAsync();
        }
        public async Task<IEnumerable<Auction>> GetByProductIdAsync(string productId)
        {
            return await _context.Auctions
                .Where(a => a.ProductId == productId)
                .ToListAsync();
        }
    }
}
