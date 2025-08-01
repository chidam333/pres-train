export interface Product {
  productId: number;
  productName: string;
  image?: string;
  price?: number;
  userName?: string;
  categoryName?: string;
  colorName?: string;
  modelName?: string;
  sellStartDate?: Date;
  sellEndDate?: Date;
  isNew?: number;
}