import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { AddProductComponent } from './add-product/add-product.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  // { path: '**', redirectTo: '' }, // Redirect to login for any unknown routes
  { path: 'add-product', component: AddProductComponent, canActivate: [AuthGuard] }, // Add route for AddProductComponent
  { path: 'product-detail/:id', component: ProductDetailComponent, canActivate: [AuthGuard]  },
  { path: 'admin-home', component: AdminHomeComponent, canActivate: [AuthGuard]  },



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
