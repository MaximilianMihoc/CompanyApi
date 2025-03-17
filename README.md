# Company Api

This is the Company API project built with .NET 9. This API provides endpoints for managing company data.

## Prerequisites
- .NET 9 SDK
- Visual Studio 2022
- SQL Server
- Git

## Getting Started
Follow these steps to clone the project and run it locally.

### Clone the Repository
git clone https://github.com/your-repo/company-api.git cd company-api

### Database Setup
Ensure you have a SQL Server instance running. Update the connection string in `appsettings.json` to point to your database.
"ConnectionStrings": {
  "CompaniesDbConnection": "Server=localhost\\MSSQLSERVER01;Database=CompaniesDb;Trusted_Connection=True;TrustServerCertificate=True"
}

### Apply Migrations
Use the following commands to create the database and apply migrations: dotnet ef database update
      
### Run the Application
Run the application using the .NET CLI: dotnet run

Alternatively, you can run the application from Visual Studio 2022 by opening the solution file and pressing `F5`.

## API Endpoints

### AuthenticationController Endpoints

1. **Register User**
   - **Endpoint:** `POST /api/authentication/register`
   - **Description:** Registers a new user.
   - **Request Body:** `UserRegistrationSubmission` (contains username, password, name, email, and optional role)
   - **Responses:**
     - `201 Created` with `CreatedResponse` if the registration is successful.
     - `400 Bad Request` with `ErrorResponse` if there is an error in the request.

2. **Login User**
   - **Endpoint:** `POST /api/authentication/login`
   - **Description:** Authenticates a user and returns a JWT token.
   - **Request Body:** `UserLoginSubmission` (contains username and password)
   - **Responses:**
     - `201 Created` with `TokenResponse` if the login is successful.
     - `400 Bad Request` with `ErrorResponse` if there is an error in the request.

3. **Refresh Token**
   - **Endpoint:** `POST /api/authentication/refresh-token`
   - **Description:** Refreshes the JWT token using a refresh token.
   - **Request Body:** `RefreshTokenSubmission` (contains userId and refreshToken)
   - **Responses:**
     - `200 OK` with `TokenResponse` if the token is refreshed successfully.
     - `400 Bad Request` with `ErrorResponse` if there is an error in the request.

4. **Check Authentication**
   - **Endpoint:** `GET /api/authentication/check-authentication`
   - **Description:** Checks if the user is authenticated.
   - **Responses:**
     - `200 OK` with a success message if the user is authenticated.

### CompanyController Endpoints

1. **Retrieve All Companies**
   - **Endpoint:** `GET /api/company`
   - **Description:** Retrieves a list of all companies.
   - **Responses:**
     - `200 OK` with a list of `CompanyResponse`.
     - `404 Not Found` with `ErrorResponse` if no companies are found.
     - `500 Internal Server Error` with `ErrorResponse` if there is a server error.

2. **Retrieve Company by ID**
   - **Endpoint:** `GET /api/company/{id}`
   - **Description:** Retrieves a specific company by its ID.
   - **Parameters:** `id` (GUID)
   - **Responses:**
     - `200 OK` with `CompanyResponse`.
     - `404 Not Found` with `ErrorResponse` if the company is not found.
     - `500 Internal Server Error` with `ErrorResponse` if there is a server error.

3. **Retrieve Company by ISIN**
   - **Endpoint:** `GET /api/company/isin/{isin}`
   - **Description:** Retrieves a specific company by its ISIN.
   - **Parameters:** `isin` (string)
   - **Responses:**
     - `200 OK` with `CompanyResponse`.
     - `404 Not Found` with `ErrorResponse` if the company is not found.
     - `500 Internal Server Error` with `ErrorResponse` if there is a server error.

4. **Add Company**
   - **Endpoint:** `POST /api/company`
   - **Description:** Adds a new company.
   - **Request Body:** `CompanySubmission` (contains company details)
   - **Responses:**
     - `201 Created` with `CreatedResponse` if the company is created successfully.
     - `400 Bad Request` with `ErrorResponse` if there is an error in the request.

5. **Update Company**
   - **Endpoint:** `PUT /api/company/{id}`
   - **Description:** Updates an existing company.
   - **Parameters:** `id` (GUID)
   - **Request Body:** `CompanySubmission` (contains updated company details)
   - **Responses:**
     - `204 No Content` if the company is updated successfully.
     - `400 Bad Request` with `ErrorResponse` if there is an error in the request.

### LookupController Endpoints

1. **Retrieve All Exchanges**
   - **Endpoint:** `GET /api/lookup/exchange`
   - **Description:** Retrieves a list of all exchanges.
   - **Responses:**
     - `200 OK` with a list of `ExchangeResponse`.
     - `500 Internal Server Error` with `ErrorResponse` if there is a server error.

2. **Retrieve Exchange by ID**
   - **Endpoint:** `GET /api/lookup/exchange/{id}`
   - **Description:** Retrieves a specific exchange by its ID.
   - **Parameters:** `id` (GUID)
   - **Responses:**
     - `200 OK` with `ExchangeResponse`.
     - `404 Not Found` with `ErrorResponse` if the exchange is not found.
     - `500 Internal Server Error` with `ErrorResponse` if there is a server error.

3. **Add Exchange**
   - **Endpoint:** `POST /api/lookup/exchange`
   - **Description:** Adds a new exchange.
   - **Request Body:** `ExchangeSubmission` (contains exchange details)
   - **Responses:**
     - `201 Created` with `CreatedResponse` if the exchange is created successfully.
     - `400 Bad Request` with `ErrorResponse` if there is an error in the request.


