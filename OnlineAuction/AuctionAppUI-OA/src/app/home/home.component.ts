import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  username: string | null = null;
  products: Product[] = [];
  filteredProducts: Product[] = [];
  searchTerm: string = '';
  minPrice: number | null = null;
  maxPrice: number | null = null;
  sortBy: string = 'price'; // Default sort option

  constructor(
    private authService: AuthService,
    private router: Router,
    private productService: ProductService
  ) { }

  ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    if (token) {
      this.username = 'User'; // Replace with actual username extraction logic if available
    }

    this.productService.getProducts().subscribe(
      (products: Product[]) => {
        this.products = products;
        this.filteredProducts = products; // Initialize with all products
      },
      error => console.error('Error fetching products', error)
    );
  }

  logout(): void {
    this.authService.logout();
    window.location.href = '/';
  }

  navigateToAddProduct(): void {
    this.router.navigate(['/add-product']);
  }

  viewProductDetail(productId: string): void {
    this.router.navigate(['/product-detail', productId]);
  }

  onSearchChange(): void {
    this.applyFiltersAndSorting();
  }

  onFilterChange(): void {
    if (this.minPrice === null && this.maxPrice === null) {
      // If both filter inputs are cleared, reset to the original product list
      this.filteredProducts = [...this.products];
    } else {
      this.applyFiltersAndSorting();
    }
  }

  onSortChange(): void {
    this.applyFiltersAndSorting();
  }

  private applyFiltersAndSorting(): void {
    let filtered = this.products.filter(product => {
      let withinPriceRange = true;
      if (this.minPrice !== null) {
        withinPriceRange = product.startingPrice >= this.minPrice;
      }
      if (this.maxPrice !== null) {
        withinPriceRange = withinPriceRange && product.startingPrice <= this.maxPrice;
      }
      return withinPriceRange;
    });

    // Apply search filter
    const lowerSearchTerm = this.searchTerm.toLowerCase();
    filtered = filtered.filter(product =>
      product.productName.toLowerCase().includes(lowerSearchTerm) ||
      product.category.toLowerCase().includes(lowerSearchTerm)
    );

    // Apply sorting and filter by remaining time
    if (this.sortBy === 'price') {
      filtered = filtered.sort((a, b) => a.startingPrice - b.startingPrice);
    } else if (this.sortBy === 'timeRemaining') {
      filtered = filtered
        .filter(product => this.calculateTimeRemaining(product) > 0) // Filter out products with no remaining time
        .sort((a, b) => this.calculateTimeRemaining(a) - this.calculateTimeRemaining(b)); // Sort by remaining time
    }

    this.filteredProducts = filtered;
  }

  private calculateTimeRemaining(product: Product): number {
    const createdAt = new Date(product.createdAt);
    const now = new Date();
    const endTime = new Date(createdAt.getTime() + product.auctionDuration * 60 * 60 * 1000);
    return Math.max(0, endTime.getTime() - now.getTime());
  }
}



// import { Component, OnInit } from '@angular/core';
// import { AuthService } from '../services/auth.service';
// import { Router } from '@angular/router';
// import { ProductService } from '../services/product.service';
// import { Product } from '../models/product.model';

// @Component({
//   selector: 'app-home',
//   templateUrl: './home.component.html',
//   styleUrls: ['./home.component.css']
// })
// export class HomeComponent implements OnInit {
//   username: string | null = null;
//   products: Product[] = [];
//   filteredProducts: Product[] = [];
//   searchTerm: string = '';

//   constructor(
//     private authService: AuthService,
//     private router: Router,
//     private productService: ProductService
//   ) { }

//   ngOnInit(): void {
//     // Fetch the token and user information when the component initializes
//     const token = localStorage.getItem('authToken');
//     if (token) {
//       // Here you might want to decode the token and extract user info
//       // For simplicity, we'll just display a message
//       this.username = 'User'; // Replace with actual username extraction logic if available
//     }

//     // Fetch products from the API
//     this.productService.getProducts().subscribe(
//       (products: Product[]) => {
//         this.products = products;
//         this.filteredProducts = products; // Initialize with all products
//       },
//       error => console.error('Error fetching products', error)
//     );
//   }

//   logout(): void {
//     this.authService.logout();
//     // Redirect to login page after logout
//     window.location.href = '/';
//   }

//   navigateToAddProduct(): void {
//     this.router.navigate(['/add-product']);
//   }

//   navigateToProductDetail(productId: string) {
//     this.router.navigate(['/product-detail', productId]);
//   }

//   viewProductDetail(productId: string): void {
//     this.router.navigate(['/product-detail', productId]);
//   }

//   onSearchChange(): void {
//     const lowerSearchTerm = this.searchTerm.toLowerCase();
//     this.filteredProducts = this.products
//       .filter(product =>
//         product.productName.toLowerCase().includes(lowerSearchTerm) ||
//         product.category.toLowerCase().includes(lowerSearchTerm)
//       )
//       .sort((a, b) => a.category.localeCompare(b.category));
//   }
// }


