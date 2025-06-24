# Medical Record Microservice

## 1. Microservice Overview

This microservice is designed for creating medical records for pets. Only authorized pet owners (authenticated via a JWT token) can add medical records. The microservice validates whether the owner has permission to create records for the pet (by comparing the `responsibleId` in the JWT token with the `responsibleId` associated with the pet).

### Microservice Flow

1. The pet owner authenticates and receives a JWT token.
2. The `responsibleId` from the token is compared with the `responsibleId` in the pet’s record.
3. If validation succeeds, the medical record is created for the pet.
4. If an error occurs (e.g., invalid token or unauthorized), an appropriate error is returned.

---

## 2. Routes and Endpoints

### `POST /medical`

**Description:** Creates a new medical record for a pet.

#### Request Body

```json
{
  "petId": 1,
  "healthStatus": "Excellent health",
  "previousDiseases": "None",
  "treatments": "Annual vaccines"
}
```

#### Response

```json
{
  "id": 123,
  "petId": 1,
  "healthStatus": "Excellent health",
  "previousDiseases": "None",
  "treatments": "Annual vaccines",
  "createdAt": "2025-06-24T12:00:00Z"
}
```

---

## 3. Microservice Functionality

### Creation Flow

- When a pet owner wants to create a medical record, a `POST` request is sent with the `petId` and medical details.
- The JWT token is validated to ensure the owner has permission to add the record for the pet.
- If valid, the record is stored in the database and returned to the client.

### Error Handling

- **Invalid Token:** If the JWT token is invalid, a 401 error is returned.
- **Missing Fields:** If the request is missing required fields, a 400 error is returned.

---

## 4. Technologies and Tools Used

- **Language:** C#
- **Framework:** ASP.NET Core
- **Database:** SQL Server
- **Authentication:** JWT
- **Swagger:** API documentation and testing

---

## 5. Authentication and Security

- **JWT:** The microservice requires a JWT token to authenticate pet owners.
- **CORS:** Configured to allow cross-origin requests.

---

## 6. Setup and Execution

Clone the repository:

```bash
git clone https://github.com/jelizalde04/medical-record.git
```

Install dependencies:

```bash
dotnet restore
```

Configure environment variables in the `.env` file.

Run the project:

```bash
dotnet run
```

---

## 7. Swagger Documentation

Access the interactive API