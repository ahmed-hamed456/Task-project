import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { ProductService } from '../../../core/services/product.service';
import { Order } from '../../../core/models/order.model';
import { Product } from '../../../core/models/product.model';
import { Observable, switchMap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.css'
})
export class OrderDetailsComponent implements OnInit {
  order$!: Observable<Order>;
  products$: Observable<Product[]>;
  productId: number | null = null;
  quantity: number | null = null;
  private orderId: number = 0;

  constructor(
    private orderService: OrderService,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.products$ = this.productService.getProducts();
  }

  ngOnInit(): void {
    // Use history.state to get order ID from navigation state (more reliable than getCurrentNavigation)
    const stateOrderId = history.state?.['orderId'];

    // Create a local observable that fetches the specific order from route params
    this.order$ = this.route.params.pipe(
      switchMap(params => {
        this.orderId = stateOrderId || +params['id'];
        return this.orderService.getOrderByIdWithoutState(this.orderId);
      })
    );
  }

  addItem(orderId: number): void {
    if (!this.productId || !this.quantity) {
      this.toastr.warning('Please select a product and enter quantity', 'Missing Information');
      return;
    }

    this.orderService.addItemToOrder(orderId, {
      productId: this.productId,
      quantity: this.quantity
    }).subscribe({
      next: () => {
        this.toastr.success('Item added to order successfully!', 'Success');
        this.productId = null;
        this.quantity = null;
        // Refresh the order display
        this.order$ = this.orderService.getOrderByIdWithoutState(this.orderId);
      }
    });
  }

  removeItem(orderId: number, productId: number): void {
    this.toastr.warning(
      'Click here to confirm removal',
      'Remove Item?',
      {
        timeOut: 5000,
        closeButton: true,
        tapToDismiss: false
      }
    ).onTap.subscribe(() => {
      this.orderService.removeItemFromOrder(orderId, productId).subscribe({
        next: () => {
          this.toastr.success('Item removed from order', 'Success');
          // Refresh the order display
          this.order$ = this.orderService.getOrderByIdWithoutState(this.orderId);
        }
      });
    });
  }

  completeOrder(orderId: number): void {
    this.toastr.info(
      'Click here to confirm. You won\'t be able to modify after completion.',
      'Complete Order?',
      {
        timeOut: 7000,
        closeButton: true,
        tapToDismiss: false
      }
    ).onTap.subscribe(() => {
      this.orderService.completeOrder(orderId).subscribe({
        next: () => {
          this.toastr.success('Order completed successfully!', 'Success');
          // Refresh the order display
          this.order$ = this.orderService.getOrderByIdWithoutState(this.orderId);
        }
      });
    });
  }

  goBack(): void {
    this.router.navigate(['/create-order']);
  }
}
