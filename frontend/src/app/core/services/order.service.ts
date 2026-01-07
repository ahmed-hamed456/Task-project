import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Order, CreateOrderRequest, AddItemToOrderRequest } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly apiUrl = `${environment.apiUrl}/orders`;
  private currentOrderSubject = new BehaviorSubject<Order | null>(null);
  public currentOrder$ = this.currentOrderSubject.asObservable();

  constructor(private http: HttpClient) {}

  createOrder(request: CreateOrderRequest): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, request).pipe(
      tap(order => this.currentOrderSubject.next(order))
    );
  }

  getOrderById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`).pipe(
      tap(order => this.currentOrderSubject.next(order))
    );
  }

  // Get order by ID without updating the shared state
  getOrderByIdWithoutState(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl);
  }

  addItemToOrder(orderId: number, request: AddItemToOrderRequest): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/${orderId}/items`, request).pipe(
      tap(order => this.currentOrderSubject.next(order))
    );
  }

  removeItemFromOrder(orderId: number, productId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${orderId}/items/${productId}`).pipe(
      tap(() => {
        // Refresh the order after removing item
        this.getOrderById(orderId).subscribe();
      })
    );
  }

  completeOrder(orderId: number): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/${orderId}/complete`, {}).pipe(
      tap(order => this.currentOrderSubject.next(order))
    );
  }

  clearCurrentOrder(): void {
    this.currentOrderSubject.next(null);
  }
}
