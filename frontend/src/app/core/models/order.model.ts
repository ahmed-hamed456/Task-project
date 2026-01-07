export interface Order {
  id: number;
  customerId: number;
  customerName: string;
  status: string;
  createdAt: string;
  items: OrderItem[];
  total: number;
  currency: string;
}

export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  currency: string;
  lineTotal: number;
}

export interface CreateOrderRequest {
  customerId: number;
}

export interface AddItemToOrderRequest {
  productId: number;
  quantity: number;
}
