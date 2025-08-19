import { Component } from '@angular/core';
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
  cartItems: any[] = [];
  wishlistItems: any[] = [];
  title = "ALTEN SHOP";

  constructor(private router: Router, private messageService: MessageService, private authService: AuthService) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
  }

  openCart() {
    this.cartVisible = true;
    this.loadCart(); // Recharger le panier à chaque ouverture
  }

  increaseQuantity(productId: any) {
    
  }

  decreaseQuantity(productId: number) {
    
  }

  updateWishlistItem(productId: number) {
    
  }

  // Méthode pour calculer le total du panier
  getCartTotal(): number {
      return this.cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  }
}