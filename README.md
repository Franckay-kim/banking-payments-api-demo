# Banking Payments API Demo

A sample ASP.NET Core Web API that demonstrates transaction intake, duplicate prevention, payment validation, JWT-secured admin endpoints, reconciliation reporting, and webhook logging.

## Features

- Payment validation endpoint
- Payment notification endpoint
- Duplicate transaction prevention
- Pending → Posted / Failed transaction lifecycle
- JWT authentication
- Transaction history
- Reconciliation summary with counts and totals
- Webhook logging
- SQLite persistence
- Swagger documentation

## Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger

## Endpoints

### Auth
- `POST /api/auth/login`

### Payments
- `POST /api/payments/validate`
- `POST /api/payments/notify`
- `GET /api/payments/transactions`
- `GET /api/payments/transactions/{reference}`

### Reconciliation
- `POST /api/reconciliation/run`

## Screenshots

### API Documentation (Swagger)

![Swagger Overview](docs/screenshots/swagger-overview.png)

### Payment Validation Request

![Validation](docs/screenshots/payment-validation-request.png)

### Payment Notification Success

![Payment Success](docs/screenshots/payment-notify-success.png)

### Duplicate Transaction Prevention

![Duplicate Prevention](docs/screenshots/duplicate-transaction.png)

### Admin Authentication

![JWT Login](docs/screenshots/auth-login-token.png)

## Architecture

![Architecture](docs/architecture.png)

## Demo Credentials

```json
{
  "username": "admin",
  "password": "Admin@123"
}