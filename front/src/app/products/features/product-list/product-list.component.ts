import { Component, OnInit, inject, signal } from "@angular/core";
import { Product } from "app/products/data-access/product.model";
import { ProductsService } from "app/products/data-access/products.service";
import { ProductFormComponent } from "app/products/ui/product-form/product-form.component";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputNumberModule } from 'primeng/inputnumber';
import { ToolbarModule } from 'primeng/toolbar';
import { InventoryStatus, ProductFilter } from "app/products/data-access/product-filter.model";
import { FormsModule } from "@angular/forms";
import { SliderModule } from 'primeng/slider';
import { AuthService } from "app/auth/data-access/auth.service";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';
import { CommonModule } from "@angular/common";
import { CartService } from "app/shared/ui/menubar/data-access/cart.service";

const emptyProduct: Product = {
  id: 0,
  code: "",
  name: "",
  description: "",
  image: "",
  category: "",
  price: 0,
  quantity: 0,
  internalReference: "",
  shellId: 0,
  inventoryStatus: "INSTOCK",
  rating: 0,
  createdAt: "",
  updatedAt: "",
};

const CATEGORY_OPTIONS: SelectItem[] = [
    { value: "Accessories", label: "Accessories" },
    { value: "Fitness", label: "Fitness" },
    { value: "Clothing", label: "Clothing" },
    { value: "Electronics", label: "Electronics" },
    { value: "All", label: "All" },
  ];
const STATUS_OPTIONS: InventoryStatus[] = ['INSTOCK', 'LOWSTOCK', 'OUTOFSTOCK'];

@Component({
  selector: "app-product-list",
  templateUrl: "./product-list.component.html",
  styleUrls: ["./product-list.component.scss"],
  standalone: true,
  imports: [DataViewModule, CardModule, ButtonModule, DialogModule, ProductFormComponent,
  InputTextModule, DropdownModule, MultiSelectModule, InputNumberModule, ToolbarModule,
  FormsModule, SliderModule, ConfirmPopupModule, ConfirmDialogModule, ToastModule, TagModule, CommonModule ],
})

export class ProductListComponent implements OnInit {
  constructor(private confirmationService: ConfirmationService, private messageService: MessageService) {}
  private readonly productsService = inject(ProductsService);
  private readonly authService = inject(AuthService);
  private readonly cartService = inject(CartService);

  public rows = 5;
  public isDialogVisible = false;
  public isCreation = false;
  public readonly editedProduct = signal<Product>(emptyProduct);

  public readonly products = this.productsService.products;
  public readonly totalRecords = this.productsService.totalCount;

  public readonly search = signal<string>('');
  public readonly filter = signal<ProductFilter>({});

  public readonly statusOptions = STATUS_OPTIONS;
  public readonly categoryOptions = CATEGORY_OPTIONS;
  
  public searchTerm = '';
  public selectedCategory: string | null = null;
  public selectedStatuses: InventoryStatus[] = [];
  public minPrice?: number;
  public maxPrice?: number;
  public sortKey: string = 'id:asc';
  public sortOptions = [
  { label: 'ID ↑', value: 'id:asc' },
  { label: 'ID ↓', value: 'id:desc' },
  { label: 'Prix ↑', value: 'price:asc' },
  { label: 'Prix ↓', value: 'price:desc' },
  { label: 'Nom A→Z', value: 'name:asc' },
  { label: 'Nom Z→A', value: 'name:desc' },
  { label: 'Catégorie A→Z', value: 'category:asc' },
  { label: 'Catégorie Z→A', value: 'category:desc' },
  { label: 'Note ↑', value: 'rating:asc' },
  { label: 'Note ↓', value: 'rating:desc' },
  { label: 'Créé récent', value: 'createdAt:desc' },
  { label: 'Créé ancien', value: 'createdAt:asc' },
  ];

  ngOnInit() {
    this.productsService.get().subscribe();
    this.reload(1, this.rows);
  }

  public onPage(event: { first: number; rows: number }) {
    const page0 = Math.floor(event.first / event.rows);
    const page1 = page0 + 1;
    this.rows = event.rows;
    this.reload(page1, this.rows);
  }
  
  public getSeverity(p: Product): 'success' | 'warning' | 'danger' {
    switch (p.inventoryStatus) {
      case 'INSTOCK': return 'success';
      case 'LOWSTOCK': return 'warning';
      default: return 'danger';
    }
  }

  // debounce pour la recherche live
  private typeTimer?: any;
  public onSearch(term: string) {
    clearTimeout(this.typeTimer);
    this.typeTimer = setTimeout(() => {
      this.search.set((term ?? '').trim());
      this.reload(1, this.rows);
    }, 300);
  }

  /** Recalcule l'objet filter() depuis l'état UI et recharge */
  public applyFilters() {
    const category = !this.selectedCategory || this.selectedCategory === 'All'
      ? undefined
      : this.selectedCategory;

    const inventoryStatus = (this.selectedStatuses?.length ?? 0) > 0
      ? this.selectedStatuses
      : undefined;

    const priceMin = typeof this.minPrice === 'number' ? this.minPrice : undefined;
    const priceMax = typeof this.maxPrice === 'number' ? this.maxPrice : undefined;

    this.filter.set({
      category,
      inventoryStatus,
      priceMin,
      priceMax
    });

    this.reload(1, this.rows);
  }

  public onResetFilters() {
    this.searchTerm = '';
    this.selectedCategory = null;
    this.selectedStatuses = [];
    this.minPrice = undefined;
    this.maxPrice = undefined;

    this.search.set('');
    this.filter.set({});

    this.reload(1, this.rows);
  }

  public onSortChange() {
    this.reload(1, this.rows);
  }

  public reload(page: number, pageSize: number) {
    this.productsService.get(page, pageSize, this.search(), this.filter(), this.sortKey).subscribe();
  }

  public isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  public onAddCart(product: Product) {
    if(product){
      this.cartService.add(product.id).subscribe();
    }
  }

  public onCreate() {
    this.isCreation = true;
    this.isDialogVisible = true;
    this.editedProduct.set(emptyProduct);
  }

  public onUpdate(product: Product) {
    this.isCreation = false;
    this.isDialogVisible = true;
    this.editedProduct.set(product);
  }

  public onDelete(product: Product) {
    this.confirmationService.confirm({
      message: 'Voulez-vous supprimer ce produit ?',
      header: `Confirmer la suppression : ${product.name} - ${product.category}`,
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass:"p-button-danger p-button-text",
      rejectButtonStyleClass:"p-button-text p-button-text",
      acceptIcon:"none",
      rejectIcon:"none",
      acceptLabel:"Oui",
      rejectLabel:"Non",

      accept: () => {
        const result = this.productsService.delete(product.id).subscribe();

        if(result) {
          this.productsService.get().subscribe();
          this.reload(1, this.rows);
          this.messageService.add({ severity: 'info', summary: 'Suppression confirmée', detail: `Produit : ${product.name} - ${product.category} supprimé avec succès !` });
        }
      }
    });
  }

  public onSave(product: Product) {
    if (this.isCreation) {
      this.productsService.create(product).subscribe();
      this.closeDialog();
    } else {
      this.productsService.update(product).subscribe();
      this.closeDialog();
    }
  }

  public onCancel() {
    this.closeDialog();
  }

  private closeDialog() {
    this.isDialogVisible = false;
  }
}
