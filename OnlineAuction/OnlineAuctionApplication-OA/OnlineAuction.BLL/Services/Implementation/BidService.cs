using OnlineAuction.BLL.Services.Interface;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuction.BLL.Services.Implementation
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<Bid> GetByIdAsync(string bidId)
        {
            return await _bidRepository.GetByIdAsync(bidId);
        }

        public async Task<IEnumerable<Bid>> GetBidsByProductIdAsync(string productId)
        {
            return await _bidRepository.GetBidsByProductIdAsync(productId);
        }

        public async Task AddAsync(Bid bid)
        {
            await _bidRepository.AddAsync(bid);
        }

        public async Task UpdateAsync(Bid bid)
        {
            await _bidRepository.UpdateAsync(bid);
        }

        public async Task DeleteAsync(string bidId)
        {
            await _bidRepository.DeleteAsync(bidId);
        }
         public async Task<Bid> GetHighestBidAsync(string productId)
        {
            return await _bidRepository.GetHighestBidAsync(productId);
        }
    }
}
