# Create Medical Record Microservice

## 1. Overview

The **Create Medical Record Microservice** is responsible for creating new medical records for pets within the Pet Medical History Management System. This microservice ensures that only authorized pet owners can create medical records for their pets through JWT token validation and database verification.

### Key Features
- Create new medical records for pets
- JWT-based authentication and authorization
- Pet ownership validation
- PostgreSQL database integration
- RESTful API design
- Swagger documentation

### Integration with Other Services
This microservice integrates with:
- **Pet Management Service**: Validates pet existence and ownership
- **Authentication Service**: Validates JWT tokens containing user credentials
- **Medical Database**: Stores medical record data
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

### POST `/medical`

**Description**: Creates a new medical record for a pet.

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
  "weight": 15.5,
  "healthStatus": "Healthy",
  "diseases": "None observed",
  "treatments": "Routine vaccination",
  "vaccinations": "Rabies, DHPP, Bordetella",
  "allergies": "None known",
  "specialCare": "Regular exercise recommended",
  "sterilized": true
}
```

**Field Descriptions**:
- `petId` (required): UUID of the pet
- `lastVisitDate` (required): Date of the veterinary visit
- `weight` (required): Pet's weight in kilograms
- `healthStatus` (required): Overall health status
- `diseases` (optional): Current or past diseases
- `treatments` (optional): Applied treatments
- `vaccinations` (required): Vaccination history
- `allergies` (optional): Known allergies
- `specialCare` (optional): Special care instructions
- `sterilized` (required): Sterilization status

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
  "message": "Not authorized to edit the medical history of this pet."
}
```

- **404 Not Found**: Pet not found
```json
{
  "message": "Pet not found."
}
```

---

## 4. Architecture & Data Models

### Models

#### Pet Model
```csharp
public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public string Image { get; set; }
    public DateTime Birthdate { get; set; }
    public string Residence { get; set; }
    public string Gender { get; set; }
    public string Color { get; set; }
    public Guid ResponsibleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

#### Medical Record Model
```csharp
public class MedicalRecord
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public DateTime LastVisitDate { get; set; }
    public float Weight { get; set; }
    public string HealthStatus { get; set; }
    public string Diseases { get; set; }
    public string Treatments { get; set; }
    public string Vaccinations { get; set; }
    public string Allergies { get; set; }
    public string SpecialCare { get; set; }
    public bool Sterilized { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Database Contexts

- **PetContext**: Manages pet data from the pet database
- **PetMedicalContext**: Manages medical records from the medical database

---

## 5. Security & Authentication

### JWT Authentication
- **Required Claims**: `userId` (GUID of the responsible user)
- **Token Validation**: 
  - No issuer validation
  - No audience validation
  - Lifetime validation enabled
  - Custom token extraction (supports both "Bearer" prefix and direct token)

### Authorization Flow
1. Extract JWT token from Authorization header
2. Validate token signature and expiration
3. Extract `userId` claim from token
4. Verify pet exists in the pet database
5. Validate that the authenticated user owns the pet
6. Create medical record if validation passes

### CORS Configuration
- Allows all origins, methods, and headers
- Configurable for production environments

---

## 6. Environment Configuration

### Required Environment Variables

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
---

## 7. API Documentation

### Swagger/OpenAPI
Interactive API documentation is available when running in development mode:

**URL**: `http://localhost:5001/api-docs-medical`
---

## 9. Usage Examples

### Creating a Medical Record with cURL

```bash
curl -X POST "http://localhost:5001/medical" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "petId": "550e8400-e29b-41d4-a716-446655440000",
    "lastVisitDate": "2024-01-15T10:30:00Z",
    "weight": 15.5,
    "healthStatus": "Healthy",
    "diseases": "None",
    "treatments": "Routine check-up",
    "vaccinations": "Up to date",
    "allergies": "None",
    "specialCare": "Regular exercise",
    "sterilized": true
  }'
```

### Creating a Medical Record with PowerShell

```powershell
$headers = @{
    "Authorization" = "Bearer YOUR_JWT_TOKEN"
    "Content-Type" = "application/json"
}

$body = @{
    petId = "550e8400-e29b-41d4-a716-446655440000"
    lastVisitDate = "2024-01-15T10:30:00Z"
    weight = 15.5
    healthStatus = "Healthy"
    diseases = "None"
    treatments = "Routine check-up"
    vaccinations = "Up to date"
    allergies = "None"
    specialCare = "Regular exercise"
    sterilized = $true
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5001/medical" -Method POST -Headers $headers -Body $body
```

---

## 9. Error Handling

The microservice implements comprehensive error handling:

### Common Error Scenarios
- **Invalid JWT Token**: Returns 401 Unauthorized
- **Missing Authorization**: Returns 401 Unauthorized
- **Pet Not Found**: Returns 404 Not Found
- **Unauthorized Pet Access**: Returns 401 Unauthorized
- **Database Connection Issues**: Returns 500 Internal Server Error
- **Invalid Request Data**: Returns 400 Bad Request

### Error Response Format
```json
{
  "message": "Descriptive error message",
  "statusCode": 400,
  "timestamp": "2024-01-15T10:30:00Z"
}
```

---

## 9. Monitoring & Logging

### Built-in Logging
The application uses ASP.NET Core's built-in logging:
- **Information**: General application flow
- **Warning**: Potential issues
- **Error**: Error conditions
- **Debug**: Detailed information (Development only)

### Log Configuration
Logging is configured in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## 10. Contributing

### Development Guidelines
1. Follow C# coding conventions
2. Ensure all endpoints are properly documented
3. Add unit tests for new functionality
4. Update this README for any API changes
5. Test with different scenarios before submitting

### Code Structure
```
create-medical/
├── Controllers/
│   └── MedicalRecordsController.cs
├── Models/
│   ├── Pet.cs
│   ├── MedicalRecord.cs
│   └── MedicalRecordCreateDto.cs
├── Data/
│   ├── PetContext.cs
│   └── PetMedicalContext.cs
├── Properties/
│   └── launchSettings.json
├── Program.cs
├── appsettings.json
├── .env
└── create-medical.csproj
```
---

## 11. License

This project is part of the Pet Medical History Management System. Please refer to the main project license for usage terms.

