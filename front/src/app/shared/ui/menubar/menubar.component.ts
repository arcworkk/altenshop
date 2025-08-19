import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from 'app/auth/data-access/auth.service';
import { MessageService } from 'primeng/api';
import { SplitterModule } from 'primeng/splitter';
import { ToolbarModule } from 'primeng/toolbar';
import { PanelMenuComponent } from '../panel-menu/panel-menu.component';
import { SidebarModule } from 'primeng/sidebar';
import { Button } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { AccordionModule } from 'primeng/accordion';
import { CartService } from './data-access/cart.service';
import { Product } from 'app/products/data-access/product.model';

@Component({
  selector: 'app-menubar',
  standalone: true,
  templateUrl: './menubar.component.html',
  styleUrls: ['./menubar.component.css'],
  imports: [ CommonModule, RouterModule, SplitterModule, ToolbarModule, PanelMenuComponent,
            SidebarModule, Button, AccordionModule],
})

export class MenubarComponent {
  cartVisible = false;
  wishlistItems: any[] = [];
  title = "ALTEN SHOP";

  public readonly cart = this.cartService.cart ?? null;
  public readonly totalRecords = this.cartService.totalItems;
  public readonly totalAmount = this.cartService.totalAmount;

  constructor(private cartService: CartService, private authService: AuthService) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.cartService.get().subscribe();
  }

  openCart() {
    this.cartVisible = true;
    this.loadCart(); // Recharger le panier à chaque ouverture
  }

  increaseQuantity(product: Product, quantity: number) {
    if(product.id) {
      quantity++
      this.cartService.update(product.id, quantity).subscribe();
      this.loadCart();
    }
  }

  decreaseQuantity(product: Product, quantity: number) {
    if(product.id && quantity == 1) {
      this.cartService.remove(product.id).subscribe();
      this.loadCart();
    }
    else {
      quantity--
      this.cartService.update(product.id, quantity).subscribe();
      this.loadCart();
    }

  }

  updateWishlistItem(productId: number) {
    
  }

  getCartTotal(): number {
      return this.cartService.totalAmount();
  }
}