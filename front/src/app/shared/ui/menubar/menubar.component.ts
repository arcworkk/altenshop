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
import { WishlistService } from './data-access/wishList.service';

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
  title = "ALTEN SHOP";

  public readonly cart = this.cartService.cart ?? null;
  public readonly wishlist = this.wishListService.wishlist ?? null;
  public readonly totalRecords = this.cartService.totalItems;
  public readonly totalAmount = this.cartService.totalAmount;

  constructor(private cartService: CartService, private wishListService: WishlistService, private authService: AuthService) {}

  ngOnInit() {
    this.loadAccordion();
  }

  loadAccordion() {
    const token = this.authService.getToken();
    if (!token){
      this.cart.set(null);
      this.wishlist.set(null);
    } else {
      this.cartService.get().subscribe();
      this.wishListService.get().subscribe();
    }
  }

  openAccordion() {
    this.cartVisible = true;
    this.loadAccordion(); // Recharger l'accordion à chaque ouverture
  }

  increaseQuantity(product: Product, quantity: number) {
    if(product.id) {
      quantity++
      this.cartService.update(product.id, quantity).subscribe();
      this.loadAccordion();
    }
  }

  decreaseQuantity(product: Product, quantity: number) {
    if(product.id && quantity == 1) {
      this.cartService.remove(product.id).subscribe();
      this.loadAccordion();
    }
    else {
      quantity--
      this.cartService.update(product.id, quantity).subscribe();
      this.loadAccordion();
    }

  }

  updateWishlistItem(product: Product) {
    if(product.id) {
      this.wishListService.update(product.id).subscribe();
      this.loadAccordion();
    }
  }

  getCartTotal(): number {
      return this.cartService.totalAmount();
  }
}