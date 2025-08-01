export interface OrderRequestDto {
  userId: number;
  customerEmail: string;
  customerPhone: string;
  customerAddress: string;
  items: {
    productId: number;
    quantity: number;
  }[];
}