using OnlineAuction.BLL.Services.Interface;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.BLL.Services.Implementation
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<Auction> GetByIdAsync(string auctionId)
        {
            return await _auctionRepository.GetByIdAsync(auctionId);
        }

        public async Task AddAsync(Auction auction)
        {
            await _auctionRepository.AddAsync(auction);
        }

        public async Task UpdateAsync(Auction auction)
        {
            await _auctionRepository.UpdateAsync(auction);
        }

        public async Task DeleteAsync(string auctionId)
        {
            await _auctionRepository.DeleteAsync(auctionId);
        }
        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _auctionRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Auction>> GetByProductIdAsync(string productId)
        {
            return await _auctionRepository.GetByProductIdAsync(productId);
        }
    }
}
