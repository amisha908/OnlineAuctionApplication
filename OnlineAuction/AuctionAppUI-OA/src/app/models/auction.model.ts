export interface AuctionResult {
    auctionId: string;
    productId: string;
    highestBidId: string;
    auctionStart: string;
    auctionEnd: string;
    highestBidAmount: number;
    highestBidderUsername: string;
    productName?: string; 
  }
  