# Pet Medical History Microservice

## 1. Summary of the Microservice

- **Purpose:**  
  The Pet Medical History Microservice handles the storage and management of medical records for pets. It allows creating and updating medical records, preserving a complete history of all changes for traceability.

- **Integration with Other Microservices:**  
  This microservice interacts with the Pet Management microservice to validate pet existence and ownership based on the responsible user (owner). It relies on JWT tokens issued by an authentication service to identify and authorize users.

---

## 2. Routes and Endpoints

### POST `/medical/update`

**Description:**  
Updates a pet’s medical record. Instead of overwriting, it always creates a new record. Fields not provided in the request retain their previous values.

**Request Headers:**

Authorization: Bearer <JWT token>
Content-Type: application/json

css
Copiar
Editar

**Request Body Example:**

```json
{
  "petId": "2811c369-f1a9-4f2e-910a-dcaa1905de5d",
  "lastVisitDate": "2025-06-26T14:30:00Z",
  "weight": 12.5,
  "healthStatus": "Healthy",
  "diseases": "None",
  "treatments": "Annual check-up",
  "vaccinations": "Rabies, Distemper",
  "allergies": "None",
  "specialCare": "None",
  "sterilized": true
}
All fields except petId are optional.

At least one field other than petId must be provided.

Successful Response (200 OK):

json
Copiar
Editar
{
  "id": "9fcd37b2-4d45-4f22-bd46-2eb9855b9867",
  "petId": "2811c369-f1a9-4f2e-910a-dcaa1905de5d",
  "lastVisitDate": "2025-06-26T14:30:00Z",
  "weight": 12.5,
  "healthStatus": "Healthy",
  "diseases": "None",
  "treatments": "Annual check-up",
  "vaccinations": "Rabies, Distemper",
  "allergies": "None",
  "specialCare": "None",
  "sterilized": true,
  "createdAt": "2025-06-26T14:35:00Z"
}
Possible Error Responses:

400 Bad Request:

json
Copiar
Editar
{
  "message": "You must provide at least one field to update."
}
401 Unauthorized:

json
Copiar
Editar
{
  "message": "Unauthorized to edit this pet's medical history."
}
404 Not Found:

json
Copiar
Editar
{
  "message": "Pet not found."
}
3. Functioning of the Microservice
The controller validates the JWT token and extracts the userId.

The microservice checks whether the pet exists and belongs to the user making the request.

If there’s no previous medical record:

A new record is created with the provided fields and default values for the rest.

If there’s an existing record:

A new medical record is created by copying all values from the latest record.

Only fields provided in the update request are overwritten.

This ensures a complete audit trail of medical history.

Dates are stored in UTC.

4. Technologies and Tools Used
Language: C#

Framework: ASP.NET Core

Database: PostgreSQL (via Entity Framework Core)

ORM: Entity Framework Core

Authentication: JWT

Documentation: Swashbuckle (Swagger)

Key NuGet Packages:

Microsoft.EntityFrameworkCore

Npgsql.EntityFrameworkCore.PostgreSQL

Microsoft.AspNetCore.Authentication.JwtBearer

Swashbuckle.AspNetCore

5. Authentication and Security
JWT tokens are required in the Authorization header for all endpoints.

The token must include the claim:

userId: GUID of the responsible user.

The microservice verifies that the requesting user is authorized to modify the pet’s medical records.

CORS can be configured to allow requests from specific origins.

6. Configuration and Running the Microservice
Prerequisites
.NET 8.0 SDK

PostgreSQL instance

Environment Variables
Variable Name	Description
ConnectionStrings__DefaultConnection	Connection string for PostgreSQL database

Installation & Execution
bash
Copiar
Editar
# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update

# Run the microservice
dotnet run
The API typically runs on:

arduino
Copiar
Editar
http://localhost:5001
7. Swagger Documentation
Swagger UI is available for interactive testing:

bash
Copiar
Editar
http://localhost:5002/api-docs-updateMedical/index.html
