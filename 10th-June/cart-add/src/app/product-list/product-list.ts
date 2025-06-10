import { Component } from '@angular/core';

@Component({
  selector: 'app-product-list',
  imports: [],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList {
  badgeCount = 0;
  increaseBadgeCount() {
    this.badgeCount++;
    console.log('Badge count increased:', this.badgeCount);
  }
}
