# Order Management System - Angular Frontend

## Overview

This is the Angular frontend for the Order Management System, demonstrating **separation of concerns**, **clean architecture principles**, and **RxJS state management** with BehaviorSubjects.

---

## Architecture

### Folder Structure

```
src/app/
├── core/                           # Core functionality (singleton services)
│   ├── interceptors/
│   │   └── error.interceptor.ts    # Global HTTP error handling
│   ├── models/                     # TypeScript interfaces/types
│   │   ├── product.model.ts
│   │   ├── order.model.ts
│   │   └── customer.model.ts
│   └── services/                   # Business services
│       ├── product.service.ts      # Product API calls + RxJS state
│       └── order.service.ts        # Order API calls + RxJS state
├── features/                       # Feature modules
│   ├── products/
│   │   └── components/
│   │       └── product-list.component.ts
│   └── orders/
│       └── components/
│           ├── create-order.component.ts
│           └── order-details.component.ts
├── shared/                         # Shared components (reusable)
│   └── components/
└── app.routes.ts                   # Application routing
```

---

## Separation of Concerns

### ✅ Components (UI Layer)
**Responsibility**: Display data and handle user interactions **ONLY**

- ✅ **Zero business logic** - Components just call services
- ✅ Use observables from services for reactive data
- ✅ Handle user input and pass to services
- ✅ Display data using templates

**Example**:
```typescript
// ✅ Good - Component just calls service
loadProducts(): void {
  this.productService.getProducts().subscribe();
}

// ❌ Bad - Business logic in component
loadProducts(): void {
  this.http.get('/api/products').subscribe(data => {
    this.products = data.filter(p => p.price > 0); // Business logic!
  });
}
```

### ✅ Services (Business Layer)
**Responsibility**: Handle API calls, state management, and business logic

- ✅ Encapsulate HTTP calls
- ✅ Manage state with **RxJS BehaviorSubject**
- ✅ Expose observables for components to subscribe to
- ✅ No DOM manipulation
- ✅ No component-specific logic

**Example**:
```typescript
private productsSubject = new BehaviorSubject<Product[]>([]);
public products$ = this.productsSubject.asObservable();

getProducts(): Observable<Product[]> {
  return this.http.get<Product[]>(this.apiUrl).pipe(
    tap(products => this.productsSubject.next(products)) // Update state
  );
}
```

### ✅ Models (Data Layer)
**Responsibility**: Define data structures

- ✅ TypeScript interfaces for type safety
- ✅ Match backend DTOs
- ✅ No logic, just data contracts

### ✅ Interceptors (Cross-Cutting Concerns)
**Responsibility**: Global HTTP handling

- ✅ Error handling
- ✅ Logging
- ✅ Authentication tokens (if needed)
- ✅ Request/response transformation

---

## RxJS State Management with BehaviorSubject

### Why BehaviorSubject?

1. **Always has a current value** - New subscribers get the latest state immediately
2. **Multicast** - Multiple components can subscribe to the same state
3. **Reactive** - Automatic updates when state changes

### Pattern Used

```typescript
// Private BehaviorSubject (internal state)
private productsSubject = new BehaviorSubject<Product[]>([]);

// Public Observable (read-only for components)
public products$ = this.productsSubject.asObservable();

// Method to update state
getProducts(): Observable<Product[]> {
  return this.http.get<Product[]>(this.apiUrl).pipe(
    tap(products => this.productsSubject.next(products))
  );
}
```

### Benefits

- ✅ Components don't manage state
- ✅ Automatic UI updates via async pipe
- ✅ Single source of truth
- ✅ Easy to debug state changes

---

## Key Features

### 1. **Product List Component**
- Displays all products from the API
- Uses `ProductService.products$` observable
- Refresh button to reload data
- **Zero business logic** - just displays service data

### 2. **Create Order Component**
- Simple form to create new order
- Validates customer ID
- Uses `OrderService.createOrder()` to call API
- Navigates to order details after creation
- **Zero business logic** - delegates to service

### 3. **Order Details Component**
- Shows order information with items
- Add/remove items functionality
- Complete order action
- Prevents modifications to completed orders (enforced by backend)
- Uses `OrderService.currentOrder$` observable
- **Zero business logic** - all validation is backend-side

---

## Error Handling

### Global Error Interceptor

```typescript
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Parse validation errors from backend
      // Display user-friendly messages
      // Log errors for debugging
    })
  );
};
```

**Features**:
- ✅ Catches all HTTP errors
- ✅ Parses backend validation errors
- ✅ Displays user-friendly messages
- ✅ Centralized error handling

---

## API Integration

### Environment Configuration

```typescript
// src/environments/environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5031/api'
};
```

### Services Use HttpClient

```typescript
constructor(private http: HttpClient) {}

getProducts(): Observable<Product[]> {
  return this.http.get<Product[]>(`${environment.apiUrl}/products`);
}
```

---

## Running the Application

### Prerequisites
- Node.js 18+ and npm
- Angular CLI installed globally (`npm install -g @angular/cli`)
- Backend API running on `http://localhost:5031`

### Development Server

```bash
cd frontend
npm install
npm start
```

Navigate to: **http://localhost:4200**

### Build for Production

```bash
npm run build
```

Build artifacts will be in `dist/` directory.

---

## Routing

| Route | Component | Description |
|-------|-----------|-------------|
| `/` | Redirects to `/products` | Default route |
| `/products` | ProductListComponent | View all products |
| `/create-order` | CreateOrderComponent | Create new order |
| `/orders/:id` | OrderDetailsComponent | View/edit order details |

---

## Testing the Application

### 1. **View Products**
1. Navigate to http://localhost:4200
2. Click "Refresh Products" to load 10 seeded products
3. Verify products display with name, description, price

### 2. **Create Order**
1. Click "Create Order" in navigation
2. Enter customer ID (1-5)
3. Click "Create Order"
4. Verify order created successfully

### 3. **Manage Order**
1. Click "View Order Details"
2. Add items by entering:
   - Product ID (1-10)
   - Quantity (any positive number)
3. Click "Add Item"
4. Verify item appears in table with calculated total
5. Remove item to test removal
6. Click "Complete Order"
7. Verify order status changes to "Completed"
8. Verify you cannot modify completed order

---

## Design Principles Applied

### 1. **Single Responsibility Principle (SRP)**
- Each component has one responsibility (display specific data)
- Each service handles one domain (products OR orders)
- Interceptor only handles errors

### 2. **Separation of Concerns**
- Components: UI only
- Services: Business logic and API calls
- Models: Data structures
- Interceptors: Cross-cutting concerns

### 3. **Dependency Injection**
- All services are injectable
- Components receive services via constructor injection
- Testable and maintainable

### 4. **Observable Pattern**
- Reactive programming with RxJS
- Components subscribe to observables
- Automatic UI updates via async pipe

### 5. **Type Safety**
- TypeScript interfaces for all data
- Compile-time type checking
- IntelliSense support

---

## Clean Architecture in Frontend

```
┌─────────────────────────────────────┐
│     Components (UI Layer)           │  ← No business logic
├─────────────────────────────────────┤
│     Services (Application Layer)    │  ← Business logic, state management
├─────────────────────────────────────┤
│     Models (Domain Layer)           │  ← Data structures
└─────────────────────────────────────┘
           ▲
           │ HTTP
┌─────────────────────────────────────┐
│     Backend API                     │  ← ASP.NET Core
└─────────────────────────────────────┘
```

### Dependency Flow
- Components depend on Services
- Services depend on Models
- Services depend on HttpClient (Angular core)
- **No circular dependencies**

---

## Comparison with Backend Architecture

| Frontend | Backend | Purpose |
|----------|---------|---------|
| Components | Controllers | Handle user input/output |
| Services | Application Services | Business logic orchestration |
| Models | DTOs | Data transfer |
| Interceptors | Middleware | Cross-cutting concerns |
| HttpClient | Repository | Data access |
| - | Domain Entities | Core business rules |

**Key Difference**: Frontend has **zero domain business rules** - all validation and business logic is in the backend. Frontend just displays data and sends requests.

---

## Best Practices Demonstrated

1. ✅ **Zero business logic in components**
2. ✅ **RxJS BehaviorSubject for state management**
3. ✅ **Global error handling with interceptors**
4. ✅ **TypeScript for type safety**
5. ✅ **Reactive programming with observables**
6. ✅ **Standalone components** (Angular 14+ best practice)
7. ✅ **Environment-based configuration**
8. ✅ **Clean folder structure** (core, features, shared)
9. ✅ **Separation of concerns** (UI vs business logic)
10. ✅ **Single responsibility** per service/component

---

## Future Enhancements

- Add unit tests with Jasmine/Karma
- Implement proper notification service (replace alerts)
- Add loading spinners
- Implement authentication
- Add form validation on frontend (complementing backend)
- Create reusable shared components (button, input, card)
- Add end-to-end tests with Cypress/Playwright
- Implement NgRx for more complex state management (if needed)

---

## Conclusion

This Angular frontend demonstrates **clean separation of concerns**, with components handling only UI responsibilities while services manage state and API communication using **RxJS BehaviorSubjects**. All business rules and validation remain in the backend, following the principle that the frontend is a **thin presentation layer**.

The architecture is scalable, testable, and maintainable, following Angular and TypeScript best practices.
