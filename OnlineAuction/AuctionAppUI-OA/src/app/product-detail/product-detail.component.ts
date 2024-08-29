import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { BidService } from '../services/bid.service'; // Import BidService
import { Product } from '../models/product.model';
import { Bid } from '../models/bid.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'; // Import FormBuilder, FormGroup, Validators
import { timer, Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { map, forkJoin } from 'rxjs';
import { AuctionService } from '../services/auction.service';
import { AuctionResult } from '../models/auction.model';
import { ChangeDetectorRef } from '@angular/core';


@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit, OnDestroy {

  product: Product | null = null;
  sellerUsername: string | null = null;
  remainingTime: string = '00:00:00';
  bidButtonActive: boolean = false;
  private countdownSubscription: Subscription | null = null;
  private auctionResultSubscription: Subscription | null = null;
  showBidSection: boolean = true; // Added flag for bid section visibility
  showAuctionResults: boolean = false; // Added flag for auction results visibility
  pId!:string;
  highestBidBelowReserve: boolean = false;
  auctionExists: boolean = false;


  userId !: string; // Replace with dynamic user ID retrieval
  bids: { bid: Bid, username: string }[] = []; 

  bidForm: Bid={
    bidId: '', // Optional because it will be auto-generated
    productId: '',
    bidderId: '',
    bidAmount: 0,
    bidTime: ''
  }

  auctionResult: AuctionResult = {
    auctionId: '',
    productId: '',
    auctionStart: '',
    auctionEnd: '',
    highestBidAmount: 0,
    highestBidderUsername: '',
    highestBidId: ''
  };
  
  

  constructor(
    private route: ActivatedRoute,
    private authService:AuthService,
    private productService: ProductService,
    private bidService: BidService, // Inject BidService
    private auctionService: AuctionService,
    private router: Router,
    private fb: FormBuilder, // Inject FormBuilder
    private cdr: ChangeDetectorRef
  ) {
   
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      let productId = params.get('id');
          console.log(productId);
      
      if (productId) {
        this.pId=productId;
        this.bidForm.productId=productId;
        this.userId=this.authService.getUserIdFromToken();
        this.bidForm.bidderId=this.userId;
        this.productService.getProductById(productId).subscribe(
          (product: Product) => {
            this.product = product;
            this.bidForm.bidAmount = product.startingPrice;
            // this.initialBid=product.startingPrice;
            if (product.sellerId) {
              this.fetchSellerUsername(product.sellerId);
            }
            this.calculateBidButtonState(product.createdAt, product.auctionDuration);
            this.fetchBids(productId); // Ensure this is called
            
            this.checkIfAuctionExists(productId); // Check if auction exists
         

             // this.displayAuctionResults();
            
          },
          error => console.error('Error fetching product details', error)
        );
      }
    });
  }

  checkIfAuctionExists(productId: string): void {
    this.auctionService.getAuctionResults(productId).subscribe(
      (result) => {
        if (result && result.length > 0) {
          this.auctionExists = true;
          this.auctionResult = result[0];
          this.displayAuctionResults();
        } else {
          this.auctionExists = false;
        }
      },
      error => console.error('Error checking auction existence', error)
    );
  }
  

  fetchSellerUsername(sellerId: string): void {
    this.productService.getSellerUsername(sellerId).subscribe(
      (response) => this.sellerUsername = response.username,
      error => console.error('Error fetching seller username', error)
    );
  }

  calculateBidButtonState(createdAt: string, auctionDuration: number): void {
    const createdAtDate = new Date(createdAt);
    const endTime = new Date(createdAtDate.getTime() + auctionDuration * 3600000); // duration in hours

    this.updateRemainingTime(endTime);

    // Update every second
    this.countdownSubscription = timer(0, 1000).subscribe(() => {
      this.updateRemainingTime(endTime);
    });
  }

  
  updateRemainingTime(endTime: Date): void {
    const now = new Date();
    const timeDiff = endTime.getTime() - now.getTime();
  
    if (timeDiff > 0) {
      this.bidButtonActive = true;
      this.showBidSection = true; // Show bid section when time is remaining
      this.showAuctionResults = false; // Hide auction results section when time is remaining
   
      const hours = Math.floor(timeDiff / (1000 * 60 * 60));
      const minutes = Math.floor((timeDiff % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((timeDiff % (1000 * 60)) / 1000);
      this.remainingTime = `${this.pad(hours)}:${this.pad(minutes)}:${this.pad(seconds)}`;
    } else {
      this.bidButtonActive = false;
      this.showBidSection = false; // Hide bid section when time is up
      this.showAuctionResults = true; // Show auction results section when time is up
    
      this.remainingTime = '00:00:00';
      if (this.countdownSubscription) {
        this.countdownSubscription.unsubscribe();
      }
      if(!this.auctionExists)
      {
        this.createAuction(); // Call createAuction when time is up'
      }
      else
      {
     
      this.displayAuctionResults();
      }
    }
  }
  
  createAuction(): void {
    if (this.product) {
      this.auctionService.createAuction(this.product.productId).subscribe(
        response => {
          console.log('Auction created successfully:', response);
          // Handle successful auction creation
          
          this.displayAuctionResults(); // Optionally, refresh auction results
          
        },
        error => {
          console.error('Error creating auction', error);
        }
      );
    }
  }
  
  displayAuctionResults(): void {
    if (this.product) {
      this.auctionService.getAuctionResults(this.pId).subscribe(
        result => {
          
          this.auctionResult = result[0];
           console.log('Auction result:', this.auctionResult); // Verify the data
           console.log(this.auctionResult.highestBidAmount);
           if(this.auctionResult.highestBidAmount==0)
           {
           this.highestBidBelowReserve=false;
           console.log(1);
           }
           else{
            console.log(2)
           this.highestBidBelowReserve = this.auctionResult.highestBidAmount > this.product!.reservedPrice;
           }
           console.log(this.highestBidBelowReserve)
          this.cdr.detectChanges(); // Ensure the UI updates
        },
        error => {
          console.error('Error fetching auction results', error);
        }
      );
    }
  }
  
  
  validateBidAmount(): void {
    const highestBidAmount = this.bids.length > 0 ? this.bids[0].bid.bidAmount : 0;
    this.bidButtonActive = this.bidForm.bidAmount > highestBidAmount;
  }

  pad(value: number): string {
    return value.toString().padStart(2, '0');
  }

  placeBid(): void {
    if (this.bidForm) {
      const bidAmount = this.bidForm.bidAmount;
      const highestBidAmount = this.bids.length > 0 ? this.bids[0].bid.bidAmount : 0;
      if (bidAmount <= highestBidAmount) {
        console.error('Bid amount must be greater than the current highest bid');
        return; // Do not proceed if bid amount is not valid
      }

      const bid: Bid = {
        bidId:'',
        productId: this.product?.productId ?? '',
        bidderId: this.userId, // Replace with actual logged-in user ID
        bidAmount: bidAmount,
        bidTime: new Date().toISOString()
      };
  
      console.log('Placing bid:', bid); // Log payload to check structure
  
      this.bidService.placeBid(bid).subscribe(
        response => {
          console.log('Bid placed successfully:', response);
          if (this.product?.productId) {
            this.fetchBids(this.product.productId);
          }
          // Handle successful bid placement
        },
        error => {
          console.error('Error placing bid', error);
        }
      );
    }
  }

  fetchBids(productId: string): void {
    this.bidService.getBidsByProductId(productId).subscribe(
      (bids: Bid[]) => {
        console.log('Fetched bids:', bids); // Log fetched bids
  
        if (bids.length > 0) {
          // Find the highest bid
          const highestBid = bids.reduce((max, bid) => bid.bidAmount > max.bidAmount ? bid : max, bids[0]);
          this.bids = [{ bid: highestBid, username: 'Fetching...' }]; // Initialize with highest bid
  
          // Fetch usernames for the highest bid
          this.authService.getUsernameById(highestBid.bidderId).subscribe(
            (response: { username: string }) => {
              this.bids[0].username = response.username;
            },
            (error: any) => console.error('Error fetching highest bid username', error)
          );
        } else {
          this.bids = [];
        }
      },
      (error: any) => console.error('Error fetching bids', error)
    );
  }
  
  

  backtoHome(): void {
    this.router.navigate(['/home']);
  }

  ngOnDestroy(): void {
    if (this.countdownSubscription) {
      this.countdownSubscription.unsubscribe();
    }
    if (this.auctionResultSubscription) {
      this.auctionResultSubscription.unsubscribe();
    }
  }
}
