import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ProductList } from './product-list/product-list';
import { UserDetails } from './user-details/user-details';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ProductList, UserDetails],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'cart-add';
}
