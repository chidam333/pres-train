
import { Component, inject, computed } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { ShoppingCartService } from './services/shopping-cart.service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet, 
    RouterLink
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private cartService = inject(ShoppingCartService);
  
  cartItemCount = computed(() => {
    return this.cartService.cart().reduce((total, item) => total + item.quantity, 0);
  });
}
