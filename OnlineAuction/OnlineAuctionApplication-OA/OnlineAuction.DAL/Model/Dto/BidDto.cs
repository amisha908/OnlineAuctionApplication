using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuction.DAL.Model.Dto
{
    public class BidDto
    {
        public string BidId { get; set; }
        public string ProductId { get; set; }
        public string BidderId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }

        // Optional: Include only necessary fields for simpler input/output
        
    }
}
