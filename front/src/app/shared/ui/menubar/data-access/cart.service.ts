import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'environments/environment';
import { AuthService } from 'app/auth/data-access/auth.service';
import { MessageService } from 'primeng/api';
import { map, catchError, tap, of } from 'rxjs';
import { ApiResult } from 'app/auth/data-access/apiResult.model';
import { Product } from 'app/products/data-access/product.model';

export interface CartItemDto {
  id: number;
  cartId: number;
  productId: number;
  quantity: number;
  product: Product;
  createdAt: string;
  updatedAt: string;
}

export interface CartDto {
  id: number;
  userId: number;
  items: CartItemDto[] | null;
  createdAt: string;
  updatedAt: string;
}

/** Payloads */
export interface CartItemAddDto { productId: number; quantity?: number; }
export interface CartItemUpdateDto { quantity?: number; }

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly http = inject(HttpClient);
  private readonly authService = inject(AuthService);
  private readonly messageService = inject(MessageService);

  private readonly API_URL = `${environment.API_BASE_URL}/cart`;

  /** State */
  public readonly cart = signal<CartDto | null>(null);
  public readonly items = computed<CartItemDto[]>(() => this.cart()?.items ?? []);
  public readonly totalItems = computed<number>(() =>
    this.items().reduce((sum, it) => sum + (it.quantity ?? 0), 0)
  );
  public readonly totalAmount = computed<number>(() =>
    this.items().reduce((sum, it) => sum + (it.product?.price ?? 0) * (it.quantity ?? 0), 0)
  );

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  /** GET /api/cart : récupère (ou crée) le panier de l’utilisateur */
  public get() {
    return this.http.get<ApiResult<CartDto>>(this.API_URL, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(res => {
        if (!res?.success) throw new Error(res?.error || 'Load cart failed');
        return res.data;
      }),
      tap(cart => this.cart.set(cart)),
      catchError(err => {
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: `Impossible de récupérer le panier.`, life: 5000 });
        throw err;
      })
    );
  }

  /** POST /api/cart : ajoute (ou incrémente) un produit */
  public add(productId: number, quantity: number = 1) {
    const payload: CartItemAddDto = { productId, quantity };
    return this.http.post<ApiResult<CartDto>>(this.API_URL, payload, { headers: this.getAuthHeaders() }).pipe(
      map(res => {
        if (!res?.success) throw new Error(res?.error || 'Add to cart failed');
        return res.data;
      }),
      tap(cart => {
        this.cart.set(cart);
        this.messageService.add({ severity: 'success', summary: 'Panier', detail: `Produit ajouté au panier.` });
      }),
      catchError(err => {
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: `Ajout au panier échoué.` });
        throw err;
      })
    );
  }

  /** PUT /api/cart/{productId} : met à jour la quantité */
  public update(productId: number, quantity: number) {
    const body: CartItemUpdateDto = { quantity };
    return this.http.put<ApiResult<CartDto>>(`${this.API_URL}/${productId}`, body, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(res => {
        if (!res?.success) throw new Error(res?.error || 'Update cart item failed');
        return res.data;
      }),
      tap(cart => {
        this.cart.set(cart);
        this.messageService.add({ severity: 'success', summary: 'Panier', detail: 'Quantité mise à jour.' });
      }),
      catchError(err => {
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: `Mise à jour de la quantité échouée.` });
        throw err;
      })
    );
  }

  /** DELETE /api/cart/{productId} : supprime un item du panier */
  public remove(productId: number) {
    return this.http.delete<ApiResult<boolean>>(`${this.API_URL}/${productId}`, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(res => {
        if (!res?.success) throw new Error(res?.error || 'Remove cart item failed');
        return res.data;
      }),
      tap(ok => {
        if (ok) {
            this.get().subscribe();
            this.messageService.add({ severity: 'info', summary: 'Panier', detail: 'Produit retiré du panier.' });
        }
      }),
      catchError(err => {
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: `Suppression du produit échouée.` });
        throw err;
      })
    );
  }

  /** DELETE /api/cart : vide le panier */
  public clear() {
    return this.http.delete<ApiResult<boolean>>(this.API_URL, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(res => {
        if (!res?.success) throw new Error(res?.error || 'Clear cart failed');
        return res.data;
      }),
      tap(ok => {
        if (ok) {
          this.cart.set(null);
          this.messageService.add({ severity: 'info', summary: 'Panier', detail: 'Panier effacé.' });
        }
      }),
      catchError(err => {
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: `Impossible d'effacer le panier.` });
        throw err;
      })
    );
  }
}