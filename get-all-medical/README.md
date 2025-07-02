# All Medical Records Microservice

## 1. Overview

The **All Medical Records Microservice** retrieves the complete medical history for a specific pet, returning all medical records in chronological order. This microservice ensures that only authorized pet owners can access the complete medical history for their pets through JWT token validation and ownership verification.

### Key Features
- Retrieve complete medical history for a pet
- Chronological ordering of records
- JWT-based authentication and authorization
- Pet ownership validation
- PostgreSQL database integration
- RESTful API design
- Swagger documentation

### Integration with Other Services
This microservice integrates with:
- **Pet Management Service**: Validates pet existence and ownership
- **Authentication Service**: Validates JWT tokens containing user credentials
- **Medical Database**: Retrieves all medical record data
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

### GET `/medical/all/{petId}`

**Description**: Retrieves all medical records for a specific pet, ordered chronologically from newest to oldest.

**Authentication**: Required (JWT Bearer token)

**Request Headers**:
```
Authorization: Bearer <JWT_TOKEN>
```

**Path Parameters**:
- `petId` (required): UUID of the pet

**Success Response (200 OK)**:
```json
[
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
  },
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "petId": "550e8400-e29b-41d4-a716-446655440000",
    "lastVisitDate": "2024-01-01T09:00:00Z",
    "weight": 15.5,
    "healthStatus": "Healthy",
    "diseases": "None observed",
    "treatments": "Routine vaccination",
    "vaccinations": "Rabies, DHPP, Bordetella",
    "allergies": "None known",
    "specialCare": "Regular exercise recommended",
    "sterilized": true,
    "createdAt": "2024-01-01T09:05:00Z"
  }
]
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

- **200 OK**: No medical records found (returns empty array)
```json
[]
```

---

## 4. Data Structure

### Medical Record Response Format
Each medical record in the response contains:

- `id`: Unique identifier for the medical record
- `petId`: UUID of the pet this record belongs to
- `lastVisitDate`: Date of the veterinary visit
- `weight`: Pet's weight in kilograms
- `healthStatus`: Overall health assessment
- `diseases`: Current or past diseases
- `treatments`: Applied treatments
- `vaccinations`: Vaccination history
- `allergies`: Known allergies
- `specialCare`: Special care instructions
- `sterilized`: Sterilization status
- `createdAt`: Timestamp when the record was created

### Ordering
- Records are ordered by `createdAt` timestamp in descending order (newest first)
- This provides a chronological view of the pet's medical history
- Most recent updates appear at the top of the array

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
cd all-medical/all-medical
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

## 6. API Documentation

### Swagger/OpenAPI
Interactive API documentation is available at:

**URL**: `http://localhost:5005/api-docs-allMedical`

---

## 7. Usage Examples

### Getting All Medical Records with cURL

```bash
curl -X GET "http://localhost:5005/medical/all/550e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Getting All Medical Records with PowerShell

```powershell
$headers = @{
    "Authorization" = "Bearer YOUR_JWT_TOKEN"
}

Invoke-RestMethod -Uri "http://localhost:5005/medical/all/550e8400-e29b-41d4-a716-446655440000" -Method GET -Headers $headers
```

### Processing Results with PowerShell

```powershell
$records = Invoke-RestMethod -Uri "http://localhost:5005/medical/all/550e8400-e29b-41d4-a716-446655440000" -Method GET -Headers $headers

# Display record count
Write-Host "Total medical records: $($records.Count)"

# Display latest record
if ($records.Count -gt 0) {
    $latestRecord = $records[0]
    Write-Host "Latest visit: $($latestRecord.lastVisitDate)"
    Write-Host "Current weight: $($latestRecord.weight) kg"
    Write-Host "Health status: $($latestRecord.healthStatus)"
}
```

---

## 8. Performance Considerations

### Database Optimization
- Records are indexed by `petId` for efficient querying
- Ordering by `createdAt` utilizes database-level sorting
- Connection pooling optimizes database performance

### Response Size Management
- For pets with extensive medical histories, consider implementing pagination
- Current implementation returns all records, suitable for most use cases
- Monitor response times for pets with very large medical histories

---

## 9. Use Cases

### Veterinary Applications
- Complete medical history review before appointments
- Treatment progression tracking
- Vaccination schedule monitoring
- Health trend analysis

### Pet Owner Access
- Comprehensive health record viewing
- Historical treatment tracking
- Preparation for veterinary visits
- Health insurance documentation

### Medical Research
- Longitudinal health studies
- Treatment effectiveness analysis
- Disease progression tracking
- Population health statistics

---

## 10. Security Considerations

### Authentication & Authorization
- JWT token validation for every request
- Pet ownership verification before data access
- Secure database connection strings
- CORS configuration for web applications

### Data Privacy
- Medical records only accessible to authorized pet owners
- No cross-user data leakage
- Secure transmission over HTTPS (in production)
- Audit logging for access tracking
