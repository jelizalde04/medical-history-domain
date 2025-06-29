# Pet Medical History Domain

## 1. Domain Overview

The **Pet Medical History Management Domain** provides a comprehensive microservices architecture for managing pet medical records. This domain enables pet owners to maintain complete medical histories for their pets through a secure, JWT-authenticated system with full CRUD operations and audit trail capabilities.

### Core Functionality

The domain manages the complete lifecycle of pet medical records, from initial creation to historical tracking, ensuring data integrity and maintaining comprehensive audit trails for veterinary care management.

---

## 2. Microservices Architecture

The domain consists of 5 independent microservices, each handling specific aspects of medical record management:

### 2.1 Create Medical Record Service
- **Port**: 5001
- **Endpoint**: `POST /medical`
- **Purpose**: Creates new medical records for pets
- **Swagger**: `http://localhost:5001/api-docs-medical`

### 2.2 Update Medical Record Service
- **Port**: 5002
- **Endpoint**: `POST /medical/update`
- **Purpose**: Updates existing medical records while preserving history
- **Swagger**: `http://localhost:5002/api-docs-updateMedical`

### 2.3 Delete Medical Record Service
- **Port**: 5005
- **Endpoint**: `DELETE /medical/{id}`
- **Purpose**: Removes specific medical records
- **Swagger**: `http://localhost:5005/api-docs-deleteMedical`

### 2.4 Latest Medical Record Service
- **Port**: 5004
- **Endpoint**: `GET /medical/latest/{petId}`
- **Purpose**: Retrieves the most recent medical record for a pet
- **Swagger**: `http://localhost:5004/api-docs-latestMedical`

### 2.5 All Medical Records Service
- **Port**: 5003
- **Endpoint**: `GET /medical/all/{petId}`
- **Purpose**: Retrieves complete medical history for a pet
- **Swagger**: `http://localhost:5003/api-docs-allMedical`

---

## 3. Technology Stack

### Core Technologies
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

## 4. Domain Folder Structure

```
medical-domain/
├── create-medical/
│   ├── create-medical/
│   │   ├── Controllers/
│   │   │   └── MedicalRecordsController.cs
│   │   ├── Models/
│   │   │   ├── Pet.cs
│   │   │   ├── MedicalRecord.cs
│   │   │   └── MedicalRecordCreateDto.cs
│   │   ├── Data/
│   │   │   ├── PetContext.cs
│   │   │   └── PetMedicalContext.cs
│   │   ├── Program.cs
│   │   ├── create-medical.csproj
│   │   └── Dockerfile
│   └── README.md
├── update-medical/
│   ├── update-medical/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Data/
│   │   ├── Program.cs
│   │   ├── update-medical.csproj
│   │   └── Dockerfile
│   └── README.md
├── delete-medical/
│   ├── delete-medical/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Data/
│   │   ├── Program.cs
│   │   ├── delete-medical.csproj
│   │   └── Dockerfile
│   └── README.md
├── latest-medical/
│   ├── latest-medical/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Data/
│   │   ├── Program.cs
│   │   ├── latest-medical.csproj
│   │   └── Dockerfile
│   └── README.md
├── all-medical/
│   ├── all-medical/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Data/
│   │   ├── Program.cs
│   │   ├── all-medical.csproj
│   │   └── Dockerfile
│   └── README.md
└── README.md (this file)
```

---

## 5. Authentication & Security

### JWT Authentication
All microservices require JWT authentication with the following specifications:

- **Token Format**: Bearer token in Authorization header
- **Required Claims**: `userId` (GUID of the pet owner)
- **Validation**: 
  - Signature validation using shared secret
  - Expiration time validation
  - Pet ownership verification

### Security Features
- **Pet Ownership Validation**: Ensures users can only access their pets' data
- **CORS Configuration**: Configurable for different environments
- **Database Security**: Secure connection strings and environment variables
- **Input Validation**: Comprehensive request validation

---

## 6. Database Architecture

### Database Structure
The domain uses two PostgreSQL databases:

#### 7.1 Pet Database (`PET_DB_NAME`)
- Stores pet information and ownership data
- Used for authentication and authorization
- Contains pet profiles and responsible user associations

#### 7.2 Medical Database (`MEDICAL_DB_NAME`)
- Stores all medical records
- Maintains complete audit trail
- Supports historical data preservation

### Data Relationships
- **One-to-Many**: Pet → Medical Records
- **Many-to-One**: Medical Records → Pet Owner
- **Audit Trail**: Historical records maintained through timestamps

---

## 7. Environment Configuration

### Required Environment Variables

Create a `.env` file in each microservice directory:

```env
# Database Configuration
DB_HOST=your-database-host
DB_USER=your-database-user
DB_PASSWORD=your-database-password
DB_PORT=5432

# Database Names
PET_DB_NAME=petdb
MEDICAL_DB_NAME=you-db-name

# JWT Configuration
JWT_SECRET=your-jwt-secret-key
```



## 8. Installation & Setup

### Prerequisites
- **.NET 8.0 SDK** or later
- **PostgreSQL** database server
- **Git** for cloning the repository
- **Docker** (optional, for containerized deployment)

### Quick Start

1. **Clone the repository**:
```bash
git clone https://github.com/jelizalde04/medical-record.git
cd medical-domain
```

2. **Set up environment variables for each service**:
```bash
# Create .env files in each microservice directory
cp .env.example create-medical/create-medical/.env

# Edit each .env file with your actual values
```

3. **Install dependencies and run each service**:
```bash

cd create-medical/create-medical
dotnet restore
dotnet run &

```

### Docker Deployment

Each microservice includes a Dockerfile for containerized deployment:

```bash
# Build and run with Docker Compose
docker-compose up --build
```

---

## 11. API Documentation

### Swagger Documentation URLs

Each microservice provides interactive API documentation:

- **Create Medical**: `http://localhost:5001/api-docs-medical`
- **Update Medical**: `http://localhost:5002/api-docs-updateMedical`
- **Delete Medical**: `http://localhost:5005/api-docs-deleteMedical`
- **Latest Medical**: `http://localhost:5004/api-docs-latestMedical`
- **All Medical**: `http://localhost:5003/api-docs-allMedical`

### Testing with Swagger
1. Navigate to any Swagger URL
2. Click "Authorize" and enter your JWT token
3. Test endpoints interactively
4. View request/response schemas and examples

---

## 12. Business Logic & Data Flow

### Medical Record Lifecycle

1. **Creation**: New medical records are created with complete pet information
2. **Updates**: Updates create new records while preserving history
3. **Retrieval**: Latest or complete history can be accessed
4. **Deletion**: Specific records can be removed when necessary

### Data Integrity Features

- **Audit Trail**: Complete history preservation through timestamped records
- **Authorization**: Strict pet ownership validation
- **Data Validation**: Comprehensive input validation and error handling
- **Transaction Safety**: Database operations with proper error handling

---


## 14. Contributing

### Development Guidelines
1. Follow C# coding conventions and best practices
2. Ensure proper JWT authentication for all endpoints
3. Maintain comprehensive error handling
4. Update documentation for any API changes
5. Include unit tests for new functionality
6. Test with various scenarios and edge cases

### Code Standards
- Use consistent naming conventions
- Implement proper exception handling
- Follow RESTful API design principles
- Maintain clean separation of concerns
- Document all public APIs and methods

---

## 15. Support & Documentation

### Additional Resources
- Individual microservice READMEs for detailed implementation
- Swagger documentation for interactive API testing
- Database schema documentation
- Deployment guides for different environments

### Contact Information
For questions, issues, or contributions, please refer to the project repository or contact the development team.

---

## 16. License

This project is part of the Pet Medical History Management System. Please refer to the project license for usage terms and conditions.