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
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IProductService _productService;
        private readonly IBidService _bidService;
        private readonly IAuthService _authService;

        public AuctionController(IAuctionService auctionService, IProductService productService, IBidService bidService, IAuthService authService)
        {
            _auctionService = auctionService;
            _productService = productService;
            _bidService = bidService;
            _authService = authService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAllAsync()
        {
            var auctions = await _auctionService.GetAllAsync(); // You need to implement this method in the service and repository
            return Ok(auctions);
        }

        // POST: api/Auction/create
        [HttpPost("create")]
        public async Task<ActionResult<AuctionDto>> CreateAuction([FromBody] CreateAuctionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ProductId))
            {
                return BadRequest("ProductId is required.");
            }

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var auctionDuration = product.AuctionDuration;
            var duration = TimeSpan.FromHours(auctionDuration);
            var auctionEnd = product.CreatedAt.Add(duration);

            var highestBid = await _bidService.GetHighestBidAsync(request.ProductId);
            if (highestBid == null)
            {
                var auction = new Auction
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = request.ProductId,
                    HighestBidId = "",
                    AuctionStart = product.CreatedAt,
                    AuctionEnd = auctionEnd,
                    HighestBidAmount = 0
                };
                await _auctionService.AddAsync(auction);

                // Create DTO and populate fields
                var auctionDto = new AuctionDto
                {
                    AuctionId = auction.AuctionId,
                    ProductId = auction.ProductId,
                    HighestBidId = auction.HighestBidId,
                    AuctionStart = auction.AuctionStart,
                    AuctionEnd = auction.AuctionEnd,
                    HighestBidAmount = auction.HighestBidAmount,
                    HighestBidderUsername = highestBid != null ? await _authService.GetUsernameByIdAsync(highestBid.BidderId) : null
                };
                return Ok(auctionDto);


            }
            else
            {
                var auction = new Auction
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = request.ProductId,
                    HighestBidId = highestBid?.BidId,
                    AuctionStart = product.CreatedAt,
                    AuctionEnd = auctionEnd,
                    HighestBidAmount = highestBid?.BidAmount ?? 0
                };
                await _auctionService.AddAsync(auction);

                // Create DTO and populate fields
                var auctionDto = new AuctionDto
                {
                    AuctionId = auction.AuctionId,
                    ProductId = auction.ProductId,
                    HighestBidId = auction.HighestBidId,
                    AuctionStart = auction.AuctionStart,
                    AuctionEnd = auction.AuctionEnd,
                    HighestBidAmount = auction.HighestBidAmount,
                    HighestBidderUsername = highestBid != null ? await _authService.GetUsernameByIdAsync(highestBid.BidderId) : null
                };
                return Ok(auctionDto);

            }

            

           
        }
     

        // GET: api/Auction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auction>> GetByIdAsync(string id)
        {
            var auction = await _auctionService.GetByIdAsync(id);

            if (auction == null)
            {
                return NotFound();
            }

            return Ok(auction);
        }

        [HttpGet("byProduct/{productId}")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetByProductIdAsync(string productId)
        {
            var auctions = await _auctionService.GetByProductIdAsync(productId);

            if (auctions == null || !auctions.Any())
            {
                return NotFound("No auctions found for the given product ID.");
            }

            var auctionDtos = new List<AuctionDto>();

            foreach (var auction in auctions)
            {
                // Fetch the highest bid asynchronously
                var highestBid = await _bidService.GetHighestBidAsync(auction.ProductId);

                // Create AuctionDto
                var auctionDto = new AuctionDto
                {
                    AuctionId = auction.AuctionId,
                    ProductId = auction.ProductId,
                    HighestBidId = auction.HighestBidId,
                    AuctionStart = auction.AuctionStart,
                    AuctionEnd = auction.AuctionEnd,
                    HighestBidAmount = auction.HighestBidAmount,
                    HighestBidderUsername = highestBid != null ? await _authService.GetUsernameByIdAsync(highestBid.BidderId) : null
                };

                auctionDtos.Add(auctionDto);
            }

            return Ok(auctionDtos);
        }

    }
}
