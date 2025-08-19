import { Injectable, inject, signal } from "@angular/core";
import { Product } from "./product.model";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { catchError, map, Observable, of, tap } from "rxjs";
import { environment } from "environments/environment";
import { ApiResult } from "app/auth/data-access/apiResult.model";
import { PaginatedResult } from './paginedResult.model';
import { ProductFilter } from "./product-filter.model";
import { AuthService } from "app/auth/data-access/auth.service";
import { MessageService } from "primeng/api";

type ProductCreateDto = Omit<Product,
  'id' | 'createdAt' | 'updatedAt'>;

function toCreateDto(p: Product): ProductCreateDto {
  const { id, createdAt, updatedAt, ...rest } = p;
  return rest;
}

@Injectable({
    providedIn: "root"
}) export class ProductsService {
    private readonly authService = inject(AuthService);
    private readonly messageService = inject(MessageService);
    private readonly API_URL = `${environment.API_BASE_URL}/products`;
    private readonly http = inject(HttpClient);
    
    private readonly _products = signal<Product[]>([]);

    public readonly products = signal<Product[]>([]);
    public readonly totalCount = signal(0);

    private page = 1;
    private pageSize = 5;
    private query = '';
    private filter: ProductFilter | null = null;
    private sort = 'id:asc';

    private getAuthHeaders(): HttpHeaders {
        const token = this.authService.getToken();
        return new HttpHeaders({
        Authorization: `Bearer ${token}`
        });
    }

    public get(page = this.page, pageSize = this.pageSize, q: string = this.query, filter = this.filter, sort = this.sort) {
        this.page = page; this.pageSize = pageSize; this.query = q; this.filter = filter; this.sort = sort;

        let params = new HttpParams()
        .set('page', String(page))
        .set('pageSize', String(pageSize))
        .set('sort', String(sort));

        if (q) {
            params = params.set('q', q);
        }

        if (filter && Object.keys(filter).length) {
            // on passe un JSON compact dans la query string
            params = params.set('filter', JSON.stringify(filter));
        }

        return this.http.get<ApiResult<PaginatedResult<Product>>>(this.API_URL, { params }).pipe(
            map(res => {
                if (!res?.success) throw new Error(res?.error || 'Load products failed');

                return res.data;
            }),
            tap(p => {
                this.products.set(p.items);
                this.totalCount.set(p.total);
            })
        );
    }

    public create(product: Product): Observable<Product> {
        return this.http.post<ApiResult<Product>>(this.API_URL, toCreateDto(product), {
            headers: this.getAuthHeaders().set('Content-Type', 'application/json')
        }).pipe(
            map(res => {
                if (res.success === true)
                {
                    this.messageService.add({ severity: 'info', summary: 'Ajout confirmée', detail: `Produit : ${product.name} - ${product.category} ajouté avec succès !` });
                }
                
                return res.data
            }),
            tap(() => this.get(this.page, this.pageSize, this.query, this.filter!).subscribe()),
            // gestion propre des erreurs de validation
            catchError(err => {
                const errs = err?.error?.errors as Record<string, string[]> | undefined;
                if (errs) {
                    const details = Object.entries(errs)
                    .map(([k, v]) => `${k}: ${v.join(', ')}`).join(' | ');
                    this.messageService.add({ severity: 'error', summary: 'Validation', detail: details, life: 5000 });
                } else {
                    this.messageService.add({ severity: 'error', summary: 'Erreur', detail: 'Création échouée' });
                }
                throw err;
            })
        );
    }

    public update(product: Product): Observable<Product> {
        return this.http.put<ApiResult<Product>>(`${this.API_URL}/${product.id}`, product, { headers: this.getAuthHeaders() }).pipe(
            map(res => {
                if (res.success === true)
                {
                    this.messageService.add({ severity: 'info', summary: 'Modification confirmée', detail: `Produit : ${product.name} - ${product.category} modifié avec succès !` });
                }
                
                return res.data
            }),
            tap(() => this.get(this.page, this.pageSize, this.query, this.filter!).subscribe()),
            // gestion propre des erreurs de validation
            catchError(err => {
                const errs = err?.error?.errors as Record<string, string[]> | undefined;
                if (errs) {
                    const details = Object.entries(errs)
                    .map(([k, v]) => `${k}: ${v.join(', ')}`).join(' | ');
                    this.messageService.add({ severity: 'error', summary: 'Validation', detail: details, life: 5000 });
                } else {
                    this.messageService.add({ severity: 'error', summary: 'Erreur', detail: 'Création échouée' });
                }
                throw err;
            })
        );
    }

    public delete(productId: number): Observable<boolean> {
        return this.http.delete<ApiResult<boolean>>(`${this.API_URL}/${productId}`, { headers: this.getAuthHeaders() }).pipe(
            map(res => {
                if (!res.success)
                {
                    this.messageService.add({ severity: 'error', summary: 'Rejeté', detail: `Erreur d'ajout : ${res.error}` });
                }
                
                return res.data
            }),
            tap(() => this.get(this.page, this.pageSize, this.query).subscribe())
        );
    }
}