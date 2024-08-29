import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../services/product.service';
import { AuthService } from '../services/auth.service';
import { Product } from '../models/product.model';
import { Router } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {

  // productForm: FormGroup;
  productForm: Product={
    productId: '',
    productName: '',
    description: '',
    startingPrice: 0,
    reservedPrice: 0,
    auctionDuration: 0,
    category: '',
    sellerId: '',
    createdAt: ''
  }

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    console.log('Submitting product:', this.productForm);

  }

  onSubmit() {
    console.log('Submitting product:', this.productForm);
    if (this.productForm) {
      const product: Product = {
        ...this.productForm,
        sellerId: this.getSellerId(), // Extract sellerId from auth service or token
        createdAt: new Date().toISOString()
       

      };

      this.productService.addProduct(product).subscribe({
        
        next: (response) => {
          this.toastr.success('Product added successfully!', 'Success');
          setTimeout(() => {
            this.router.navigate(['/home']);
          }, 3000); // Redirect after 3 seconds
        },
        error: (err) => {
          this.toastr.error('Error adding product!', 'Error');
        }
      });
    }
  }
  backtoHome() {
    this.router.navigate(['/home']);

  }

  private getSellerId(): string {
    const token = localStorage.getItem('authToken');
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    }
    return '';
  }
}
