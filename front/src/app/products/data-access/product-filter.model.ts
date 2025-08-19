export type InventoryStatus = 'INSTOCK' | 'LOWSTOCK' | 'OUTOFSTOCK';

export type ProductFilter = Partial<{
  category: string;
  inventoryStatus: InventoryStatus[]; // multi
  priceMin: number;
  priceMax: number;
  ratingMin: number;
  ratingMax: number;
}>;