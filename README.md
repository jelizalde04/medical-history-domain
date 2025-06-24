# Pet Medical History Domain

## 1. Domain Overview

The Pet Medical History Management Domain provides infrastructure for registering, managing, and querying pet medical records. It allows pet owners (authenticated via a JWT token) to create medical records for their pets, including details about health status, past diseases, and treatments received.

### Involved Microservices

- **Medical Record Creation Microservice:** Allows pet owners to create medical records for pets.
---

## 2. Technologies Used

- **Language:** C# (.NET Core)
- **Framework:** ASP.NET Core
- **Database:** SQL Server (stores medical records and pet information)
- **Authentication:** JWT (JSON Web Tokens) for validating and authorizing pet owners
- **Swagger:** Interactive documentation for testing API endpoints
- **ORM:** Entity Framework Core for database interaction

---

## 3. Domain Folder Structure

The domain is organized by independent microservices, each responsible for its own functionality:

```
Controllers/
  ├── MedicalRecordsController.cs   // Handles medical record 
Models/
  ├── MedicalRecord.cs             // Represents medical records
  ├── Pet.cs                       // Represents pet information
Data/
  ├── PetMedicalContext.cs         // Context for medical records
  └── PetContext.cs                // Context for pet information

Program.cs                         // Main configuration of the microservices
```

---

## 4. Setup and Execution

### Installation

Clone the repository:

```bash
git clone https://github.com/jelizalde04/medical-record.git
```

Install dependencies:

```bash
dotnet restore
```

Configure environment variables:

Create a `.env` file and add necessary credentials (e.g., database credentials, `JWT_SECRET`):

```ini
DB_HOST=localhost
DB_USER=user
DB_PASSWORD=password
DB_PORT=5432
JWT_SECRET=my-secret-key
```

Run the project:

```bash
dotnet run
```

---

## 5. Authentication and Security

- **JWT:** The microservice requires a JWT token to authenticate pet owners.
- **CORS:** Configured to allow requests from different domains.