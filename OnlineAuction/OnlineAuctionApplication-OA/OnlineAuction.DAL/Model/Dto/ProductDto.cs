public class ProductDto
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal ReservedPrice { get; set; }
    public double AuctionDuration { get; set; } // Duration in hours
    public string Category { get; set; }
    public string SellerId { get; set; }
    public DateTime CreatedAt { get; set; }
}
