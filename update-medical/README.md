# Update Medical Record Microservice

## 1. Overview

The **Update Medical Record Microservice** handles updating existing medical records for pets. Instead of overwriting existing records, it creates new records with updated information while preserving the complete medical history for audit trail purposes.

### Key Features
- Update pet medical records with history preservation
- JWT-based authentication and authorization
- Pet ownership validation
- PostgreSQL database integration
- Complete audit trail maintenance
- RESTful API design
- Swagger documentation


### Integration with Other Services
This microservice integrates with:
- **Pet Management Service**: Validates pet existence and ownership
- **Authentication Service**: Validates JWT tokens containing user credentials
- **Medical Database**: Stores updated medical record data
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

### POST `/medical/update`

**Description**: Updates a pet's medical record by creating a new record with updated values while preserving the history.

**Authentication**: Required (JWT Bearer token)

**Request Headers**:
```
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json
```

**Request Body**:
```json
{
  "petId": "550e8400-e29b-41d4-a716-446655440000",
  "lastVisitDate": "2024-01-15T10:30:00Z",
  "weight": 16.0,
  "healthStatus": "Good",
  "diseases": "Mild arthritis",
  "treatments": "Pain management medication",
  "vaccinations": "Rabies, DHPP, Bordetella (updated)",
  "allergies": "Chicken protein",
  "specialCare": "Limited exercise, soft diet",
  "sterilized": true
}
```

**Field Requirements**:
- `petId` (required): UUID of the pet
- At least one other field must be provided for update
- All other fields are optional and will retain previous values if not provided

**Success Response (200 OK)**:
```json
{
  "id": "456e7890-e89b-12d3-a456-426614174000",
  "petId": "550e8400-e29b-41d4-a716-446655440000",
  "lastVisitDate": "2024-01-15T10:30:00Z",
  "weight": 16.0,
  "healthStatus": "Good",
  "diseases": "Mild arthritis",
  "treatments": "Pain management medication",
  "vaccinations": "Rabies, DHPP, Bordetella (updated)",
  "allergies": "Chicken protein",
  "specialCare": "Limited exercise, soft diet",
  "sterilized": true,
  "createdAt": "2024-01-15T10:35:00Z"
}
```

**Error Responses**:

- **400 Bad Request**: No fields provided for update
```json
{
  "message": "You must provide at least one field to update."
}
```

- **401 Unauthorized**: Invalid or missing JWT token
```json
{
  "message": "Unauthorized to edit this pet's medical history."
}
```

- **404 Not Found**: Pet not found
```json
{
  "message": "Pet not found."
}
```

---

## 4. Business Logic

### Update Process
1. **Authentication**: Validate JWT token and extract user ID
2. **Authorization**: Verify pet exists and belongs to the authenticated user
3. **History Preservation**: Retrieve the latest medical record for the pet
4. **Record Creation**: Create a new medical record with:
   - Previous values for fields not included in the update
   - New values for fields included in the update
   - New creation timestamp
5. **Database Storage**: Save the new record to maintain complete audit trail

### Data Integrity
- All dates stored in UTC format
- Complete medical history preserved
- No data overwriting - only new record creation
- Audit trail maintained through creation timestamps

---

## 5. Installation & Setup

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
cd update-medical/update-medical
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

4. **Apply database migrations** (if available):
```bash
dotnet ef database update
```

5. **Run the application**:
```bash
dotnet run
```

The service will start on `http://localhost:5002`

---

## 6. API Documentation

### Swagger/OpenAPI
Interactive API documentation is available at:

**URL**: `http://localhost:5002/api-docs-updateMedical`

---

## 7. Usage Examples

### Updating a Medical Record with cURL

```bash
curl -X POST "http://localhost:5002/medical/update" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "petId": "550e8400-e29b-41d4-a716-446655440000",
    "weight": 16.0,
    "healthStatus": "Good",
    "treatments": "Updated treatment plan"
  }'
```

### Updating a Medical Record with PowerShell

```powershell
$headers = @{
    "Authorization" = "Bearer YOUR_JWT_TOKEN"
    "Content-Type" = "application/json"
}

$body = @{
    petId = "550e8400-e29b-41d4-a716-446655440000"
    weight = 16.0
    healthStatus = "Good"
    treatments = "Updated treatment plan"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5002/medical/update" -Method POST -Headers $headers -Body $body
```