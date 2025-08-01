
import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ShoppingCartComponent } from './components/shopping-cart/shopping-cart.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { ColorListComponent } from './components/color-list/color-list.component';
import { NewsListComponent } from './components/news-list/news-list.component';
import { ContactUsComponent } from './components/contact-us/contact-us.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
  { path: '', component: ProductListComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'cart', component: ShoppingCartComponent },
  { path: 'categories', component: CategoryListComponent },
  { path: 'colors', component: ColorListComponent },
  { path: 'news', component: NewsListComponent },
  { path: 'contact', component: ContactUsComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '' } // Wildcard route for 404 page
];
