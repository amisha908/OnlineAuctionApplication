using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Model.Domain
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal ReservedPrice { get; set; }
        public double AuctionDuration { get; set; }
        public string Category { get; set; }
        public string SellerId { get; set; } // Foreign key to ApplicationUser
        public DateTime CreatedAt { get; set; }

        public ApplicationUser Seller { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
