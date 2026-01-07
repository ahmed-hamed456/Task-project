import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { CustomerService } from '../../../core/services/customer.service';
import { Customer } from '../../../core/models/customer.model';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-order.component.html',
  styleUrl: './create-order.component.css'
})
export class CreateOrderComponent implements OnInit {
  customerId: number | null = null;
  createdOrderId: number | null = null;
  customers$: Observable<Customer[]>;

  constructor(
    private orderService: OrderService,
    private customerService: CustomerService,
    private router: Router,
    private toastr: ToastrService,
    private cdr: ChangeDetectorRef
  ) {
    this.customers$ = this.customerService.getCustomers();
  }

  ngOnInit(): void {
    this.orderService.clearCurrentOrder();
  }

  createOrder(): void {
    if (!this.customerId) {
      this.toastr.warning('Please select a customer', 'Missing Information');
      return;
    }

    this.orderService.createOrder({ customerId: this.customerId }).subscribe({
      next: (order) => {
        this.createdOrderId = order.id;
        this.cdr.detectChanges();
        this.toastr.success(`Order #${order.id} created successfully!`, 'Success', {
          timeOut: 5000
        });
      }
    });
  }

  viewOrder(): void {
    if (this.createdOrderId) {
      this.router.navigate(['/orders', this.createdOrderId], {
        state: { orderId: this.createdOrderId }
      });
    }
  }
}
