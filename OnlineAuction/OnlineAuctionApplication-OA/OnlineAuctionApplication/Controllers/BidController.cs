using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineAuction.BLL.Services.Interface;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.DAL.Model.Dto;
using System;
using System.Threading.Tasks;

namespace OnlineAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly IMapper _mapper;

        public BidController(IBidService bidService, IMapper mapper)
        {
            _bidService = bidService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] BidDto bidDto)
        {
            var bid = _mapper.Map<Bid>(bidDto);
            bid.BidId = Guid.NewGuid().ToString();
            bid.BidTime = DateTime.Now;
            await _bidService.AddAsync(bid);
            return Ok(bid);

            // Ensure the action name and route parameter are correctly matched
            // return CreatedAtAction(nameof(GetByIdAsync), new { bidId = bid.BidId }, bidDto);
        }

        [HttpGet("{bidId}")]
        public async Task<IActionResult> GetByIdAsync(string bidId)
        {
            var bid = await _bidService.GetByIdAsync(bidId);
            if (bid == null) return NotFound();
            return Ok(_mapper.Map<BidDto>(bid));
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetBidsByProductIdAsync(string productId)
        {
            var bids = await _bidService.GetBidsByProductIdAsync(productId);
            if (bids == null || !bids.Any()) return NotFound();
            return Ok(_mapper.Map<IEnumerable<BidDto>>(bids));
        }

    }

}
