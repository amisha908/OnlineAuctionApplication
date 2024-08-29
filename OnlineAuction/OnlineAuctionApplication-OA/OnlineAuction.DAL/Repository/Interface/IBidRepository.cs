using OnlineAuction.DAL.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Repository.Interface
{
    public interface IBidRepository
    {
        Task<Bid> GetByIdAsync(string bidId);
        Task<IEnumerable<Bid>> GetBidsByProductIdAsync(string productId);
        Task AddAsync(Bid bid);
        Task UpdateAsync(Bid bid);
        Task DeleteAsync(string bidId);
        Task<Bid> GetHighestBidAsync(string productId);
    }
}
