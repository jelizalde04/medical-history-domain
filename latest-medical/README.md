# Latest Medical Record Microservice

## 1. Overview

The **Latest Medical Record Microservice** retrieves the most recent medical record for a specific pet. This microservice ensures that only authorized pet owners can access medical records for their pets through JWT token validation and ownership verification.

### Key Features
- Retrieve latest medical record for a pet
- JWT-based authentication and authorization
- Pet ownership validation
- PostgreSQL database integration
- RESTful API design
- Swagger documentation

### Integration with Other Services
This microservice integrates with:
- **Pet Management Service**: Validates pet existence and ownership
- **Authentication Service**: Validates JWT tokens containing user credentials
- **Medical Database**: Retrieves medical record data
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

### GET `/medical/latest/{petId}`

**Description**: Retrieves the most recent medical record for a specific pet.

**Authentication**: Required (JWT Bearer token)

**Request Headers**:
```
Authorization: Bearer <JWT_TOKEN>
```

**Path Parameters**:
- `petId` (required): UUID of the pet

**Success Response (200 OK)**:
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "petId": "550e8400-e29b-41d4-a716-446655440000",
  "lastVisitDate": "2024-01-15T10:30:00Z",
  "weight": 15.5,
  "healthStatus": "Healthy",
  "diseases": "None observed",
  "treatments": "Routine vaccination",
  "vaccinations": "Rabies, DHPP, Bordetella",
  "allergies": "None known",
  "specialCare": "Regular exercise recommended",
  "sterilized": true,
  "createdAt": "2024-01-15T10:35:00Z"
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
  "message": "Not authorized to view the medical history of this pet."
}
```

- **404 Not Found**: Pet not found
```json
{
  "message": "Pet not found."
}
```

- **404 Not Found**: No medical records found
```json
{
  "message": "No medical records found for this pet."
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
PET_DB_NAME=petdb
MEDICAL_DB_NAME=medicaldb
```

### Installation Steps

1. **Clone the repository**:
```bash
git clone <repository-url>
cd latest-medical/latest-medical
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

The service will start on `http://localhost:5004`

---

## 5. API Documentation

### Swagger/OpenAPI
Interactive API documentation is available at:

**URL**: `http://localhost:5004/api-docs-latestMedical`

---

## 6. Usage Examples

### Getting Latest Medical Record with cURL

```bash
curl -X GET "http://localhost:5004/medical/latest/550e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Getting Latest Medical Record with PowerShell

```powershell
$headers = @{
    "Authorization" = "Bearer YOUR_JWT_TOKEN"
}

Invoke-RestMethod -Uri "http://localhost:5004/medical/latest/550e8400-e29b-41d4-a716-446655440000" -Method GET -Headers $headers
```
