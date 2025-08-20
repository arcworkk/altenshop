import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'environments/environment';
import { AuthService } from 'app/auth/data-access/auth.service';
import { MessageService } from 'primeng/api';
import { map, tap, catchError } from 'rxjs';
import { ApiResult } from 'app/auth/data-access/apiResult.model';
import { Product } from 'app/products/data-access/product.model';

export interface WishlistItemDto {
  id: number; wishlistId: number; productId: number;
  product: Product; createdAt: string; updatedAt: string;
}
export interface WishlistDto {
  id: number; userId: number; items: WishlistItemDto[] | null;
  createdAt: string; updatedAt: string;
}

@Injectable({ providedIn: 'root' })
export class WishlistService {
  private readonly http = inject(HttpClient);
  private readonly auth = inject(AuthService);
  private readonly toast = inject(MessageService);
  private readonly API_URL = `${environment.API_BASE_URL}/wishlist`;

  public readonly wishlist = signal<WishlistDto | null>(null);

  private getAuthHeaders(): HttpHeaders {
    const token = this.auth.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  get() {
    return this.http.get<ApiResult<WishlistDto>>(this.API_URL, { headers: this.getAuthHeaders() }).pipe(
      map(res => { if (!res?.success) throw new Error(res?.error || 'Load wishlist failed'); return res.data; }),
      tap(w => this.wishlist.set(w))
    );
  }

  // POST /api/wishlist/{productId} (pas de body selon le Swagger)
  add(productId: number) {
    return this.http.post<ApiResult<WishlistDto>>(`${this.API_URL}/${productId}`, null, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(res => { if (!res?.success) throw new Error(res?.error || 'Add wishlist failed'); return res.data; }),
      tap(w => { this.wishlist.set(w); this.toast.add({ severity: 'success', summary: 'Wishlist', detail: 'Ajouté à la liste d’envie.' }); })
    );
  }

  // PUT /api/wishlist/{productId}
  update(productId: number) {
    return this.http.put<ApiResult<WishlistDto>>(`${this.API_URL}/${productId}`, null, {
      headers: this.getAuthHeaders()
    }).pipe(
      map(res => { if (!res?.success) throw new Error(res?.error || 'Update wishlist failed'); return res.data; }),
      tap(w => { this.wishlist.set(w); this.toast.add({ severity: 'info', summary: 'Wishlist', detail: 'Mise à jour effectuée.' }); })
    );
  }

  // DELETE /api/wishlist
  clear() {
    return this.http.delete<ApiResult<boolean>>(this.API_URL, { headers: this.getAuthHeaders() }).pipe(
      map(res => { if (!res?.success) throw new Error(res?.error || 'Clear wishlist failed'); return res.data; }),
      tap(ok => { if (ok) this.wishlist.set(null); })
    );
  }
}