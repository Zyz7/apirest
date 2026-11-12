# ApiRest — ASP.NET Core 9

<img width="1916" height="953" alt="Screenshot124725" src="https://github.com/user-attachments/assets/2eb2f86e-08f5-403e-892d-ff5ca0c0eb96" />

A REST API built with **C#**, **ASP.NET Core 9**, **Entity Framework Core** and **Swagger**, featuring **JWT authentication**, and full CRUD for products, categories, suppliers, addresses, users and logs.

🔗 **Live demo (Swagger UI):** [https://apirest-yogs.onrender.com](https://apirest-yogs.onrender.com)

---

## ✨ Features

- 🔐 **JWT authentication** — a token is issued on login and unlocks all protected endpoints.
- 🛒 **Full CRUD** for products, categories, suppliers and addresses.
- 👤 User registration, login and password modification.
- 📋 Read-only log endpoint for auditing records.
- 📖 **Interactive documentation** with Swagger UI served at the root path.
- 🌍 CORS enabled (`AllowEverything` policy).
- 🗄️ MySQL database (Pomelo provider), with a SQLite fallback option.
- 🐳 Docker support and `render.yaml` for deployment to Render.

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 (ASP.NET Core Web API) |
| ORM | Entity Framework Core |
| Database | MySQL (Pomelo provider) / SQLite (optional) |
| Auth | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Docs | Swashbuckle (Swagger / OpenAPI) |
| Deployment | Docker, Render |

## 📂 Project Structure

```
apirest/
├── controllers/        # API controllers (Product, Category, Supplier, Address, User, Login, Log)
├── models/             # EF Core entities
├── dtos/               # Data Transfer Objects for requests/responses
├── repositories/       # Data access layer
├── config/             # Configuration helpers
├── Program.cs          # App setup: DI, JWT, CORS, Swagger
├── appsettings.json    # Connection string & JWT settings
├── apirest.sql         # SQL schema
├── Dockerfile
└── render.yaml
```

## 📦 Installation

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- MySQL Server (or Docker)

### Project setup

```bash
# Create the project
dotnet new webapi -n ApiEjemploMySQL
cd ApiEjemploMySQL

# Entity Framework Core with MySQL
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
dotnet add package Microsoft.Data.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.*
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.*
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.0
```

Or simply clone this repository and run:

```bash
git clone https://github.com/Zyz7/apirest.git
cd apirest
dotnet restore
dotnet run
```

Then open **http://localhost:5054** (or the port shown in the console) to explore the Swagger UI.

## ⚙️ Configuration

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DBConnection": "Server=localhost;Port=3306;Database=apirest;User=root;Password=<your-password>;"
  },
  "Jwt": {
    "Key": "<your-secret-key>",
    "Issuer": "<your-issuer>",
    "Audience": "<your-audience>"
  }
}
```

| Variable | Description |
|---|---|
| `ConnectionStrings:DBConnection` | MySQL connection string (SQLite path if using the SQLite provider) |
| `Jwt:Key` | Secret key used to sign the tokens (HMAC-SHA256) |
| `Jwt:Issuer` / `Jwt:Audience` | Token issuer and audience (both validated) |
| `PORT` | Optional. Overrides the default port |

## 🔄 Migrations

Create the initial migration and apply it to the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Requires the `Microsoft.EntityFrameworkCore.Tools` and `Microsoft.EntityFrameworkCore.Design` packages (already included above).

## 🐳 Docker

```bash
# Build the image
docker build -t apirest .

# Run the container
docker run -p 3000:3000 apirest
```

The API will be available at **http://localhost:3000**.

## 🔑 Authentication

Send your credentials to `POST /api/login` and you'll receive a JWT token:

```json
{ "email": "user@example.com", "password": "your-password" }
```

Use the token on protected endpoints via the `Authorization` header:

```
Authorization: Bearer <token>
```

In Swagger UI, click the **Authorize** button and paste the token to test protected endpoints.

> ℹ️ All `GET` endpoints and user registration are public. Create, update and delete operations require a valid token.

## 📡 Endpoints

Base URL: `http://localhost:3000` *(Docker)* or `http://localhost:5054` *(local run)*

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/login` | Log in and get a JWT token | ❌ Public |
| GET | `/api/user` | Get users | ❌ Public |
| POST | `/api/user` | Register a new user | ❌ Public |
| PUT | `/api/user/{id}` | Update user / modify password | ✅ JWT |
| GET | `/api/product` | List all products (with their category) | ❌ Public |
| GET | `/api/product/{id}` | Get a product by ID | ❌ Public |
| POST | `/api/product` | Create a product | ✅ JWT |
| PUT | `/api/product/{id}` | Update a product | ✅ JWT |
| DELETE | `/api/product/{id}` | Delete a product | ✅ JWT |
| GET | `/api/category` | List categories | ❌ Public |
| GET | `/api/category/{id}` | Get a category by ID | ❌ Public |
| POST | `/api/category` | Create a category | ✅ JWT |
| PUT | `/api/category/{id}` | Update a category | ✅ JWT |
| DELETE | `/api/category/{id}` | Delete a category | ✅ JWT |
| GET | `/api/supplier` | List suppliers (with their address) | ❌ Public |
| GET | `/api/supplier/{id}` | Get a supplier by ID | ❌ Public |
| POST | `/api/supplier` | Create a supplier | ✅ JWT |
| PUT | `/api/supplier/{id}` | Update a supplier | ✅ JWT |
| DELETE | `/api/supplier/{id}` | Delete a supplier | ✅ JWT |
| GET | `/api/address` | List addresses | ❌ Public |
| GET | `/api/address/{id}` | Get an address by ID | ❌ Public |
| POST | `/api/address` | Create an address | ✅ JWT |
| PUT | `/api/address/{id}` | Update an address | ✅ JWT |
| DELETE | `/api/address/{id}` | Delete an address | ✅ JWT |
| GET | `/api/log` | List logs | ✅ JWT |
| GET | `/api/log/{id}` | Get a log by ID | ✅ JWT |

### Example — create a product

```http
POST /api/product
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Wireless Mouse",
  "description": "Ergonomic wireless mouse",
  "amount": 10,
  "price": 19.99,
  "categoryId": 1
}
```

**Responses:** `201 Created` · `401 Unauthorized` (missing/invalid token) · `404 Not Found` (unknown product or category)

## ☁️ Deployment (Render)

The repository includes a `render.yaml` blueprint. On Render, choose **New → Blueprint** and point it to this repository — the service will deploy from the included `Dockerfile`.

> ⚠️ On Render's free tier the service sleeps after ~15 minutes of inactivity, so the first request may take up to a minute to respond.

## 📄 License

Distributed under the [MIT License](LICENSE).
