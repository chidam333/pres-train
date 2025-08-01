import { Component, inject } from '@angular/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shopping-cart',
  template: `
    <h2 class="text-2xl font-bold mb-4">Shopping Cart</h2>
    @if(cartService.cart().length > 0) {
      <div class="border p-4 rounded-lg">
        <ul>
          @for(item of cartService.cart(); track item.product.productId) {
            <li class="flex justify-between items-center py-2 border-b">
              <div class="flex-grow">
                <span class="font-semibold">{{ item.product.productName }}</span>
                <div class="text-sm text-gray-600">
                  Price: {{ item.product.price | currency:'INR' }}
                </div>
              </div>
              <div class="flex items-center">
                <input
                  type="number"
                  [value]="item.quantity"
                  (change)="updateQuantity(item.product.productId, $event)"
                  min="1"
                  class="w-16 text-center border rounded-md mr-2"
                />
                <button (click)="cartService.removeFromCart(item.product.productId)" class="text-red-500 hover:text-red-700">Remove</button>
              </div>
            </li>
          }
        </ul>
        <div class="flex justify-between items-center mt-4">
          <span class="text-xl font-bold">Total: {{ cartService.getCartTotal() | currency:'INR' }}</span>
          <button (click)="cartService.clearCart()" class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600">Clear Cart</button>
          <button (click)="startCheckout()" class="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600">Checkout</button>
        </div>
      </div>
    } @else {
      <p>Your cart is empty.</p>
    }

    @if (showCheckoutForm) {
      <div class="mt-8 p-4 border rounded-lg">
        <h3 class="text-xl font-semibold mb-4">Checkout Information</h3>
        <form (ngSubmit)="processCheckout()">
          <div class="mb-4">
            <label for="email" class="block text-gray-700 text-sm font-bold mb-2">Email:</label>
            <input type="email" id="email" [(ngModel)]="customerEmail" name="customerEmail" required class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline">
          </div>
          <div class="mb-4">
            <label for="phone" class="block text-gray-700 text-sm font-bold mb-2">Phone:</label>
            <input type="tel" id="phone" [(ngModel)]="customerPhone" name="customerPhone" required class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline">
          </div>
          <div class="mb-4">
            <label for="address" class="block text-gray-700 text-sm font-bold mb-2">Address:</label>
            <textarea id="address" [(ngModel)]="customerAddress" name="customerAddress" required class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"></textarea>
          </div>
          <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">Place Order</button>
          <button type="button" (click)="cancelCheckout()" class="ml-2 bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600">Cancel</button>
        </form>
      </div>
    }
  `,
  imports: [CurrencyPipe, NgFor, NgIf, FormsModule]
})
export class ShoppingCartComponent {
  cartService = inject(ShoppingCartService);

  showCheckoutForm: boolean = false;
  customerEmail: string = '';
  customerPhone: string = '';
  customerAddress: string = '';

  updateQuantity(productId: number, event: Event) {
    const target = event.target as HTMLInputElement;
    const quantity = parseInt(target.value, 10);
    if (!isNaN(quantity)) {
      this.cartService.updateQuantity(productId, quantity);
    }
  }

  startCheckout() {
    this.showCheckoutForm = true;
  }

  async processCheckout() {
    try {
      const response = await this.cartService.checkout(this.customerEmail, this.customerPhone, this.customerAddress);
      alert(`Order placed successfully! Order ID: ${response.orderId}, Status: ${response.status}`);
      this.showCheckoutForm = false;
      this.customerEmail = '';
      this.customerPhone = '';
      this.customerAddress = '';
    } catch (error) {
      alert('Failed to place order. Please try again.');
    }
  }

  cancelCheckout() {
    this.showCheckoutForm = false;
  }
}
