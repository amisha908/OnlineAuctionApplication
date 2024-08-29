export interface Bid {
    bidId: string; // Optional because it will be auto-generated
    productId: string;
    bidderId: string;
    bidAmount: number;
    bidTime: string;
  }
  