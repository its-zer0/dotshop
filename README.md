# DotShop

DotShop is a modern, extensible e-commerce API built with ASP.NET Core. It provides a robust backend for managing products, orders, and user authentication, making it easy to build and scale online stores.

## Features

- **User Authentication & Authorization**

  - Register, login, change password, reset password
  - JWT-based authentication
  - Role-based access control

- **Product Management**

  - CRUD operations for products
  - Product validation and stock management

- **Order Management**

  - Place, update, and delete orders
  - Order history per user
  - Order item and total price calculation

- **API Documentation**
  - Interactive Swagger UI for testing and exploring endpoints

## Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Authentication
- Swagger/OpenAPI

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or cloud)
- (Optional) [Visual Studio Code](https://code.visualstudio.com/) or Visual Studio

### Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/DotShop.git
   cd DotShop
   ```

2. **Configure the database**

   - Update `appsettings.json` with your SQL Server connection strings for `DefaultConnection` and `AuthConnection`.

3. **Apply database migrations**

   ```bash
   dotnet ef database update --context AppDbContext
   dotnet ef database update --context AuthDbContext
   ```

4. **Run the application**

   ```bash
   dotnet run
   ```

5. **Access the API**
   - Swagger UI: [http://localhost:5279/swagger](http://localhost:5279/swagger)
   - API endpoints: `/api/products`, `/api/orders`, `/api/user/register`, etc.

## API Overview

### Authentication

- `POST /api/user/register` — Register a new user
- `POST /api/user/login` — Login and receive JWT
- `POST /api/user/forgot-password` — Request password reset
- `POST /api/user/reset-password` — Reset password

### Products

- `GET /api/products` — List all products
- `POST /api/products` — Add a new product
- `PUT /api/products/{id}` — Update a product
- `DELETE /api/products/{id}` — Delete a product

### Orders

- `GET /api/orders/my-orders` — Get orders for the current user
- `POST /api/orders` — Place a new order
- `PUT /api/orders/{id}` — Update an order
- `DELETE /api/orders/{id}` — Delete an order
