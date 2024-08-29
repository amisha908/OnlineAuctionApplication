using OnlineAuction.DAL.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Repository.Interface
{
    public interface IAuctionRepository
    {
        Task<Auction> GetByIdAsync(string auctionId);
        Task AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task DeleteAsync(string auctionId);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<IEnumerable<Auction>> GetByProductIdAsync(string productId);
    }
}
