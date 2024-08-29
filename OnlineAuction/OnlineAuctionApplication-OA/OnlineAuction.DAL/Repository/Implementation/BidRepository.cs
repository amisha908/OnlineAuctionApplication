using Microsoft.EntityFrameworkCore;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Repository.Implementation
{
    public class BidRepository : IBidRepository
    {
        private readonly OnlineAuctionDbContext _context;

        public BidRepository(OnlineAuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Bid> GetByIdAsync(string bidId)
        {
            return await _context.Bids.FindAsync(bidId);
        }

        public async Task<IEnumerable<Bid>> GetBidsByProductIdAsync(string productId)
        {
            return await _context.Bids.Where(b => b.ProductId == productId).ToListAsync();
        }

        public async Task AddAsync(Bid bid)
        {
            if (bid.BidId == null)
            {
                bid.BidId = Guid.NewGuid().ToString(); // Generate a new GUID if it's not set
            }
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bid bid)
        {
            _context.Bids.Update(bid);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string bidId)
        {
            var bid = await _context.Bids.FindAsync(bidId);
            if (bid != null)
            {
                _context.Bids.Remove(bid);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Bid> GetHighestBidAsync(string productId)
        {
            return await _context.Bids
                .Where(b => b.ProductId == productId)
                .OrderByDescending(b => b.BidAmount)
                .FirstOrDefaultAsync();
        }
    }
}
