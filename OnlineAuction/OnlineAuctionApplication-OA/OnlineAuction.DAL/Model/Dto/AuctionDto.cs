using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Model.Dto
{
    public class AuctionDto
    {
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string HighestBidId { get; set; }
        public DateTime AuctionStart { get; set; }
        public DateTime AuctionEnd { get; set; }
        public decimal HighestBidAmount { get; set; }
        public string HighestBidderUsername { get; set; }
    }
}
