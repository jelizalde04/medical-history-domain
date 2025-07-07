# Delete Medical Record Microservice

## 1. Overview

The **Delete Medical Record Microservice** is responsible for safely removing medical records from the Pet Medical History Management System. This microservice ensures that only authorized pet owners can delete medical records for their pets through JWT token validation and ownership verification.

### Key Features
- Delete existing medical records by ID
- JWT-based authentication and authorization
- Pet ownership validation
- PostgreSQL database integration
- RESTful API design
- Swagger documentation

### Integration with Other Services
This microservice integrates with:
- **Pet Management Service**: Validates pet existence and ownership
- **Authentication Service**: Validates JWT tokens containing user credentials
- **Medical Database**: Removes medical record data
- **Pet Database**: Retrieves pet information for validation

---

## 2. Technology Stack

- **Language**: C# (.NET 8.0)
- **Framework**: ASP.NET Core Web API
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Documentation**: Swagger/OpenAPI
- **Environment Management**: DotNetEnv

### Key Dependencies
```xml
<PackageReference Include="DotNetEnv" Version="3.1.1" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.17" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.6" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

---

## 3. API Endpoints

### DELETE `/medical/{id}`

**Description**: Deletes a specific medical record by ID.

**Authentication**: Required (JWT Bearer token)

**Request Headers**:
```
Authorization: Bearer <JWT_TOKEN>
```

**Path Parameters**:
- `id` (required): UUID of the medical record to delete

**Success Response (200 OK)**:
```json
{
  "message": "Medical record deleted successfully."
}
```

**Error Responses**:

- **401 Unauthorized**: Invalid or missing JWT token
```json
{
  "message": "Token not provided."
}
```

- **401 Unauthorized**: Pet ownership validation failed
```json
{
  "message": "Not authorized to delete the medical history of this pet."
}
```

- **404 Not Found**: Medical record not found
```json
{
  "message": "Medical record not found."
}
```

---

## 4. Installation & Setup

### Prerequisites
- **.NET 8.0 SDK** or later
- **PostgreSQL** database server
- **Git** for cloning the repository

### Environment Variables

Create a `.env` file in the project root:

```env
DB_HOST=your-database-host
DB_USER=your-database-user
DB_PASSWORD=your-database-password
DB_PORT=5432
JWT_SECRET=your-jwt-secret-key
PET_DB_NAME=you-db-name
MEDICAL_DB_NAME=you-db-name
```

### Installation Steps

1. **Clone the repository**:
```bash
git clone <repository-url>
cd delete-medical/delete-medical
```

2. **Install dependencies**:
```bash
dotnet restore
```

3. **Configure environment variables**:
```bash
# Create .env file with your database credentials
cp .env.example .env
# Edit .env with your actual values
```

4. **Run the application**:
```bash
dotnet run
```

The service will start on `http://localhost:5005`

---

## 5. API Documentation

### Swagger/OpenAPI
Interactive API documentation is available at:

**URL**: `http://localhost:5005/api-docs-deleteMedical`

---

## 6. Usage Examples

### Deleting a Medical Record with cURL

```bash
curl -X DELETE "http://localhost:5005/medical/123e4567-e89b-12d3-a456-426614174000" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Deleting a Medical Record with PowerShell

```powershell
$headers = @{
    "Authorization" = "Bearer YOUR_JWT_TOKEN"
}

Invoke-RestMethod -Uri "http://localhost:5005/medical/123e4567-e89b-12d3-a456-426614174000" -Method DELETE -Headers $headers
```

