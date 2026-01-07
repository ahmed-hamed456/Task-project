import { Routes } from '@angular/router';
import { ProductListComponent } from './features/products/components/product-list.component';
import { CreateOrderComponent } from './features/orders/components/create-order.component';
import { OrderDetailsComponent } from './features/orders/order-details/order-details.component';
import { OrderListComponent } from './features/orders/order-list/order-list.component';

export const routes: Routes = [
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'products', component: ProductListComponent },
  { path: 'create-order', component: CreateOrderComponent },
  { path: 'orders', component: OrderListComponent },
  { path: 'orders/:id', component: OrderDetailsComponent }
];
