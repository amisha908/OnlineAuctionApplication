import { Component, OnInit } from '@angular/core';
import { AuctionService } from '../services/auction.service';
import { AuctionResult } from '../models/auction.model';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product.model';
import { BidService } from '../services/bid.service';
import { Bid } from '../models/bid.model';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  auctions: AuctionResult[] = [];
  products: Product[] = [];
  errorMessage: string = '';
  productNames: { [key: string]: string } = {}; // Maps productId to product name
  highestBidders: { [key: string]: string } = {}; // Maps auctionId to highest bidder username

  constructor(
    private auctionService: AuctionService,
    private productService: ProductService,
    private bidService: BidService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadAuctions();
    this.loadProducts();
  }

  loadAuctions(): void {
    this.auctionService.getAuction().subscribe(
      (auctions: AuctionResult[]) => {
        this.auctions = auctions;
        this.loadProductNames();
        this.loadHighestBidders();
      },
      error => {
        console.error('Error fetching auctions', error);
        this.errorMessage = 'Failed to load auctions';
      }
    );
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe(
      (products: Product[]) => {
        this.products = products;
      },
      error => {
        console.error('Error fetching products', error);
        this.errorMessage = 'Failed to load products';
      }
    );
  }

  loadProductNames(): void {
    const productIds = this.auctions.map(a => a.productId);
    productIds.forEach(id => {
      this.productService.getProductById(id).subscribe(
        product => {
          this.productNames[id] = product.productName;
        },
        error => console.error('Error fetching product', error)
      );
    });
  }

  loadHighestBidders(): void {
    this.auctions.forEach(auction => {
      this.bidService.getBidsByProductId(auction.productId).subscribe(
        (bids: Bid[]) => {
          if (bids.length > 0) {
            const highestBid = bids.reduce((prev, current) => (prev.bidAmount > current.bidAmount) ? prev : current);
            this.authService.getUsernameById(highestBid.bidderId).subscribe(
              response => {
                this.highestBidders[auction.auctionId] = response.username;
              },
              error => console.error('Error fetching username', error)
            );
          }
        },
        error => console.error('Error fetching bids', error)
      );
    });
  }

  deleteProduct(productId: string): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(productId).subscribe(
        () => {
          this.products = this.products.filter(p => p.productId !== productId);
          console.log('Product deleted');
          this.toastr.success('Product deleted successfully!', 'Success');
         

        },
        error => {
          console.error('Error deleting product', error);
          this.errorMessage = 'Failed to delete product';
        }
      );
    }
    }
  }

