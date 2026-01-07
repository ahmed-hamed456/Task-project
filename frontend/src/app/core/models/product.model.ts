export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  currency: string;
}

export interface CreateProductRequest {
  name: string;
  description: string;
  price: number;
}
