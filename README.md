# Shetlandsponnyerna

## API Endpoints
### AuthenticationController
| Method | Endpoint | Authentication | Description |
|--------|----------|----------------|----------------|
| GET    | `/Authentication/Id` | ✅ User | Get Your Own Personal Details By Id | 
| POST   | `/Authentication/Login` | ❌ Not Authenticated | Login As User | 
| POST   | `/Authentication/Add-Hairdresser` | ✅ Admin | Create A Hairdresser | 
| POST   | `/Authentication/Register` | ❌ NOT Authenticated | Register As User | 
| PUT    | `/Authentication/Id` | ✅ Hairdresser/Admin | Update Hairdresser Details | 
| DELETE | `/Authentication/Delete` | ✅ User | Delete Your Account | 

### BookingController
| Method | Endpoint | Authentication | Description |
|--------|----------|----------------|----------------|
| GET    | `/Booking/Available-Times` | ❌ NOT Authenticated | Get A Hairdresser Available-Times |
| POST   | `/Booking/BookAppointment` | ✅ User | Book Appointment As User | 
| PUT    | `/Booking/Reschedule` | ✅ User | Reschedule Your Own Booking | 
| DELETE | `/Booking/Cancel-Booking` | ✅ User | Cancel Your Own Booking | 
| GET    | `/Booking/BookingsById` | ✅ User | Get Your Booking By Id |

### TreatmentController
| Method | Endpoint | Authentication | Description |
|--------|----------|----------------|----------------|
| GET    | `/Treatment` | ❌ Not Authenticated | Get All Treatments | 
| GET    | `/Treatment/Id` | ❌ Not Authenticated | Get Treatment By Id | 
| POST   | `/Treatment` | ✅ Admin | Create A New Treatment | 
| PUT    | `/Treatment/Id` | ✅ Admin | Update A Treatment By Id |  
| DELETE | `/Treatment/Id` | ✅ Admin | Delete A Treatment By Id | 

### UserController
| Method | Endpoint | Authentication | Description |
|--------|----------|----------------|----------------|
| GET    | `/User/Get-Users` | ✅ Hairdresser/Admin | Get All Users | Get All Users |
| GET    | `/User/Bookings-Overview` | ✅ Admin | Get All Bookings | Get All Bookings |
| GET    | `/User/Hairdresser-Week-Schedule` | ✅ Hairdresser | Get Hairdresser Week Schedule | 
| GET    | `/User/Hairdresser-Monthly-Schedule` | ✅ Hairdresser | Get Hairdresser Monthly Schedule | 
| GET    | `/User/Booking/Id` | ✅ Hairdresser/Admin | Get A Booking By Id 
| GET    | `/User/Id` | ✅ Hairdresser/Admin | Get A Hairdresser By Id | 
| GET    | `/User/Get-Hairdressers` | ❌ Not Authenticated | Get All Hairdressers | 
| PUT    | `/User/Update-User/Id` | ✅ User | Update Your Own Personal Details | 


## HairSalon Booking System 
This Project is a back-end booking system designed for hair salons, enabling them to effiently manage appointments, services, and customer information.

## Main Purpose 
The primary goal of this system is to streamline the booking process for hair salons, offering a reliable and scalable platform for managing their daily operations.
It aims to reduce administrative overhead, minimize booking errors, and provide a seamless experience for both salon staff and customers.

## Target Audience 
The system is built for hair salon owners, staff and customers, providing them with the tools to manage appointments, client data, and service offerings. While this particular project focuses on the backend, its designed to be integrated with a user-friendly frontend application for both salon administrators and customers. 

## Core Functionality 
The system provides essential functionalities for managing a hair salon's operations, including: 
* User and Authentication Management:
    * Register new user accounts and log in to access protected functionalities.
    * Retrieve their personal details by ID.
    * Delete their own account.
    * for administrators, the system allows the creation of new hairdresser accounts and updating hairdresser details.  Hairdressers also have privileges to update their own details.

* Appointment Booking and Mangement:
    * Customers can view avaiable appointment times for hairdressers.
    * They can book new appointsments, reschedule existing ones, and cancel their own bookings.
    * Users can also retrieve their booking details by ID.
 
* Service (Treatment) Management:
    * The system allows anyone to view all available treatment and get details for a specific treatment by ID.
    * Administrators have full control over treatments, being able to create new treatments, update existing ones and delte treatments.
 
* User and Schedule Overview:
    * Hairdresser and Admins can retrieve a list of all registered users.
    * Admins have access to a ful overview of all bookings in the system.
    * Hairdressers can view their weekly and monthly schedules.
    * Both hairdressers and admins can retrieve details for any specific booking by Id and get details for individual hairdressers by ID.
    * The system also allows any to get list of all hairdresser.
    * Users can update their own accounts details only (Customers). 

## Key Features 
* RESTful API: Provides a clean and intuitive API for seamless integration with client applications.
* Robust Data Management: Efficiently handels appointments,service and customer data with clear separation of concerns.
* Automated Testing: Ensures reliability and stability through comprehensive unit and integration tests. 

## Architecture Overview 
This booking system is built with a microservices-oriented structure to promote modularity, scalability, and independent deployment of components. 

* ASP.NET Web API: The core of the backend system, exposing RESTful endpoints for client applications.
* Controllers: Handle incoming HTTP requests, route them to the appropiate services, and return responses.
* Services Layer: Contains the business logic, orchestrating operations and interacting with the repository layer.
* Repository Layer: Abstracts data access operations, providing a clean interface for interacting with the database.
* DTOs (Data Transfer Objects): Used to transfer data between layers, ensuring clear data contracts and avoiding direct exposure of domain models.
* Mappers: Convert between DTOs and domain models, simplifying data transformation.
* Database The persistent storage for all application data (appointments,services, customers, etc).
* Unit Tests (MSTest): These tests focus on verifying the functionality of indiviual components (Repositories,Services) in isolation. They ensure that each part of the system works correctly on its own.
* Integration Tests (Postman): These tests verify the interaction between dirrerent components or layers of the system, including the Controllers. Postman is used to send requests to the API endpoints exposed by the Controllers and verify the responses. 

## Key Technologies and Frameworks
* Backend: ASP.NET Web API
* Language: C#
* Testing Frameworks: MSTest (unit testing), Postman (integration testing)
* Dependency Injection: Built-in ASP.NET Core DI
* Object-Relational Mapping (ORM): Entity Framework Core. 

## Teststrategi och resultat 
Projektet använder enhetstester för att verifiera funktionalitet i de viktigaste komponenterna som är:
* BookingService
* BookingRepository
* TreatmentService
* TreatmentRepository
* UserService
* UserRepository
Testerna är skrivna i MSTest och använder Moq för att isolera beroenden.
Fokus låg på att säkerställa korrekt affärslogik, datavalidering och hantering av olika scenarier både normal fall och felaktiga.
Totalt genomfördes 92 enhetstester och alla av dem blev godkända och detta visar att systemet fungerar som förväntat.
Man kan även se alla enhetstester i Testcatalog.md på github.

## Postman-tester
Utöver enhetstester genomfördes integrationstester med Postman för att verifiera att RESTful-API:et svarar korrekt.
Det testades bland annat:
* Inloggning och registrering 
* Hämta behandlingar
* Skapa, hämta och ta bort bokningar
* Roller och åtkomstbehörighet
* Edge-cases vid fel indata etc.

## API Endpoints Documentation

Describe the available API endpoints, including request and response examples. Use the format below for consistency:

### Endpoint 1 - Get All Bookings Overview

**URL:** `Api/User/Bookings-Overview`  
**Method:** GET  
**Description:** Returns a list of all bookings with details for admin users.  
**Request Parameters:**

_None_

**Response:**

```json
[
  {
    "id": 1,
    "start": "2025-06-10T14:00:00",
    "end": "2025-06-10T14:30:00",
    "customer": "Anna Svensson",
    "hairdresser": "Elin Karlsson",
    "treatment": "Klippning"
  }
]
```

### Endpoint 2 - Create New Hairdresser

**URL:** `/api/Authentication/Add-Hairdresser`  
**Method:** POST  
**Description:** Creates a new hairdresser user and assigns them the "Hairdresser" role. Only accessible by admins.  
**Request Parameters:**

* `firstName` (string) - First name of the new hairdresser  
* `lastName` (string) - Last name of the new hairdresser  
* `userName` (string) - Chosen username  
* `email` (string) - Email address  
* `phoneNumber` (string) - Phone number  
* `password` (string) - Desired password  
* `confirmPassword` (string) - Confirmation of the password

**Example Request Body:**

```json
{
  "firstName": "Elin",
  "lastName": "Karlsson",
  "userName": "elin.k",
  "email": "elin.k@example.com",
  "phoneNumber": "0701234567",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

**Response:**

```json
{
  "id": "abc123",
  "userName": "elin.k",
  "email": "elin.k@example.com",
  "phoneNumber": "0701234567",
  "role": "Hairdresser"
}
```

### Endpoint 3 - User Login

**URL:** `/api/Authentication/Login`  
**Method:** POST  
**Description:** Authenticates a user and returns a JWT token on success.  
**Request Parameters:**

* `email` (string) - User’s email address  
* `password` (string) - User’s password

**Example Request Body:**

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
}
```

### Endpoint 4 - Get Available Times

**URL:** `/api/Booking/Available-Times`  
**Method:** GET  
**Description:** Returns a list of available booking times for a hairdresser on a specific day.  
**Request Parameters:**

* `hairdresserId` (string) - ID of the hairdresser  
* `treatmentId` (integer) - ID of the treatment  
* `day` (string, date-time) - The date to check availability for (e.g., `2025-06-10T00:00:00Z`)

**Response:**

```json
[
  "2025-06-10T09:00:00",
  "2025-06-10T10:00:00",
  "2025-06-10T11:00:00"
]
```

### Endpoint 5 - Get Booking By ID

**URL:** `/api/Booking/Booking-By-Id`  
**Method:** GET  
**Description:** Returns details for a single booking by ID. Requires the user to be authenticated.  
**Request Parameters:**

* `bookingId` (integer) - The ID of the booking

**Response:**

```json
{
  "id": 1,
  "start": "2025-06-03T11:27:40.954Z",
  "end": "2025-06-03T11:57:40.954Z",
  "treatment": {
    "id": 1,
    "name": "Klippning",
    "description": "En professionell klippning.",
    "duration": 30,
    "price": 500
  },
  "costumer": {
    "id": "u1",
    "firstName": "Anna",
    "lastName": "Svensson",
    "userName": "anna123",
    "email": "anna@example.com",
    "phoneNumber": "0701234567"
  },
  "hairdresser": {
    "id": "h1",
    "firstName": "Elin",
    "lastName": "Karlsson",
    "userName": "elin.k",
    "email": "elin.k@example.com",
    "phoneNumber": "0709876543"
  }
}
```

### Endpoint 6 - Book Appointment

**URL:** `/api/Bookings/BookAppointment`  
**Method:** POST  
**Description:** Books a new appointment for a logged-in customer.  
**Request Body:**

```json
{
  "hairdresserId": "h1",
  "treatmentId": 1,
  "start": "2025-06-10T14:00:00"
}
```

**Response:**

```json
{
  "id": 42,
  "start": "2025-06-10T14:00:00",
  "end": "2025-06-10T14:30:00",
  "treatment": {
    "id": 1,
    "name": "Klippning",
    "description": "En professionell klippning.",
    "duration": 30,
    "price": 500
  },
  "costumer": {
    "id": "u1",
    "firstName": "Anna",
    "lastName": "Svensson",
    "userName": "anna123",
    "email": "anna@example.com",
    "phoneNumber": "0701234567"
  },
  "hairdresser": {
    "id": "h1",
    "firstName": "Elin",
    "lastName": "Karlsson",
    "userName": "elin.k",
    "email": "elin.k@example.com",
    "phoneNumber": "0709876543"
  }
}
```

### Endpoint 7 - Cancel Booking

**URL:** `/api/Bookings/Cancel Booking`  
**Method:** DELETE  
**Description:** Cancels an existing booking for the logged-in user.  
**Request Parameters:**

* `bookingId` (integer) - ID of the booking to cancel

**Response:**

```json
{
  "message": "Booking with ID 42 has been cancelled successfully."
}
```

### Endpoint 8 - Reschedule Booking

**URL:** `/api/Booking/Reschedule`  
**Method:** PUT  
**Description:** Reschedules an existing booking for the logged-in user.  
**Request Parameters:**

* `bookingId` (integer) - ID of the booking to update

**Request Body:**

```json
{
  "hairdresserId": "h1",
  "treatmentId": 2,
  "start": "2025-06-12T11:00:00"
}
```

**Response:**

```json
{
  "id": 42,
  "start": "2025-06-12T11:00:00",
  "end": "2025-06-12T12:00:00",
  "treatment": {
    "id": 2,
    "name": "Färgning",
    "description": "En färgning av ditt hår.",
    "duration": 60,
    "price": 800
  },
  "costumer": {
    "id": "u1",
    "firstName": "Anna",
    "lastName": "Svensson",
    "userName": "anna123",
    "email": "anna@example.com",
    "phoneNumber": "0701234567"
  },
  "hairdresser": {
    "id": "h1",
    "firstName": "Elin",
    "lastName": "Karlsson",
    "userName": "elin.k",
    "email": "elin.k@example.com",
    "phoneNumber": "0709876543"
  }
}
```

### Endpoint 9 - Get All Hairdressers

**URL:** `/api/Hairdresser`  
**Method:** GET  
**Description:** Returns a list of all users with the "Hairdresser" role.  
**Request Parameters:** _None_

**Response:**

```json
[
  {
    "id": "abc123",
    "firstName": "Elin",
    "lastName": "Karlsson",
    "userName": "elin.k",
    "email": "elin.k@example.com",
    "phoneNumber": "0709876543"
  }
]
```

### Endpoint 10 - Get Weekly Schedule

**URL:** `/api/Hairdresser/Week-Schedule`  
**Method:** GET  
**Description:** Returns a hairdresser’s weekly schedule starting from a given date.  
**Request Parameters:**

* `hairdresserId` (string) - ID of the hairdresser  
* `weekStart` (string, date-time) - Start date of the week (e.g., `2025-06-03T00:00:00Z`)

**Response:**

```json
[
  {
    "day": "2025-06-03",
    "appointments": [
      {
        "start": "2025-06-03T09:00:00",
        "end": "2025-06-03T09:30:00",
        "customerName": "Anna Svensson",
        "treatment": "Klippning"
      }
    ]
  }
]
```

### Endpoint 11 - Get Monthly Schedule

**URL:** `/api/Hairdresser/Monthly-Schedule`  
**Method:** GET  
**Description:** Returns a hairdresser’s full schedule for the specified month and year.  
**Request Parameters:**

* `hairdresserId` (string) - ID of the hairdresser  
* `year` (integer) - The year (e.g., 2025)  
* `month` (integer) - The month (1–12)

**Response:**

```json
[
  {
    "date": "2025-06-05",
    "appointments": [
      {
        "start": "2025-06-05T13:00:00",
        "end": "2025-06-05T13:45:00",
        "treatment": "Färgning",
        "customer": "Anna Svensson"
      }
    ]
  }
]
```

### Endpoint 12 - Update Hairdresser

**URL:** `/api/Authentication/Id`  
**Method:** PUT  
**Description:** Updates the profile of a hairdresser by ID.  
**Request Parameters:**

* `id` (string) - ID of the hairdresser (in path)

**Request Body:**

```json
{
  "firstName": "Elin",
  "lastName": "Karlsson",
  "userName": "elin.k",
  "email": "elin.k@example.com",
  "phoneNumber": "0709876543"
}
```

**Response:**

```json
{
  "message": "Hairdresser updated successfully."
}
```

### Endpoint 13 - Get Hairdresser By ID

**URL:** `/api/Hairdresser/{id}`  
**Method:** GET  
**Description:** Returns detailed profile info including bookings for a specific hairdresser.  
**Request Parameters:**

* `id` (string) - Hairdresser ID

**Response:**

```json
{
  "id": "h1",
  "firstName": "Elin",
  "lastName": "Karlsson",
  "userName": "elin.k",
  "email": "elin.k@example.com",
  "phoneNumber": "0709876543",
  "bookings": [
    {
      "id": 1,
      "start": "2025-06-03T11:27:40.954Z",
      "end": "2025-06-03T11:57:40.954Z",
      "treatment": {
        "id": 1,
        "name": "Klippning",
        "description": "...",
        "duration": 30,
        "price": 500
      },
      "customer": {
        "id": "u1",
        "firstName": "Anna",
        "lastName": "Svensson",
        "userName": "anna123",
        "email": "anna@example.com",
        "phoneNumber": "0701234567"
      }
    }
  ]
}
```

### Endpoint 14 - Get Booking Details (Hairdresser)

**URL:** `/api/Hairdresser/booking/{id}`  
**Method:** GET  
**Description:** Returns detailed information for a single booking by ID.  
**Request Parameters:**

* `id` (integer) - Booking ID

**Response:**

```json
{
  "id": 1,
  "start": "2025-06-03T11:27:40.954Z",
  "end": "2025-06-03T11:57:40.954Z",
  "treatment": {
    "id": 1,
    "name": "Klippning",
    "description": "En professionell klippning.",
    "duration": 30,
    "price": 500
  },
  "costumer": {
    "id": "u1",
    "firstName": "Anna",
    "lastName": "Svensson",
    "userName": "anna123",
    "email": "anna@example.com",
    "phoneNumber": "0701234567"
  },
  "hairdresser": {
    "id": "h1",
    "firstName": "Elin",
    "lastName": "Karlsson",
    "userName": "elin.k",
    "email": "elin.k@example.com",
    "phoneNumber": "0709876543"
  }
}
```

### Endpoint 15 - Get All Treatments

**URL:** `/api/Treatment`  
**Method:** GET  
**Description:** Retrieves a list of all treatments available in the system.  
**Request Parameters:** _None_

**Response:**

```json
[
  {
    "id": 1,
    "name": "Klippning",
    "description": "En professionell klippning",
    "duration": 60,
    "price": 500
  }
]
```

### Endpoint 16 - Create New Treatment

**URL:** `/api/Treatment`  
**Method:** POST  
**Description:** Creates a new treatment and stores it in the database.  
**Request Body:**

```json
{
  "name": "Färgning",
  "description": "Färgning av hår",
  "duration": 90,
  "price": 800
}
```

**Response:**

```json
{
  "id": 2,
  "name": "Färgning",
  "description": "Färgning av hår",
  "duration": 90,
  "price": 800
}
```

### Endpoint 17 - Get Treatment by ID

**URL:** `/api/Treatment/{id}`  
**Method:** GET  
**Description:** Returns a specific treatment based on its ID.  
**Request Parameters:**

* `id` (integer) - ID of the treatment

**Response:**

```json
{
  "id": 1,
  "name": "Klippning",
  "description": "En professionell klippning",
  "duration": 60,
  "price": 500
}
```

### Endpoint 18 - Update Treatment

**URL:** `/api/Treatment/{id}`  
**Method:** PUT  
**Description:** Updates an existing treatment.  
**Request Parameters:**

* `id` (integer) - ID of the treatment to update

**Request Body:**

```json
{
  "name": "Uppdaterad behandling",
  "description": "Ny beskrivning",
  "duration": 45,
  "price": 600
}
```

**Response:**

```json
{
  "message": "Treatment updated successfully."
}
```

### Endpoint 19 - Delete Treatment

**URL:** `/api/Treatment/{id}`  
**Method:** DELETE  
**Description:** Deletes a treatment by its ID.  
**Request Parameters:**

* `id` (integer) - ID of the treatment to delete

**Response:**

```json
{
  "message": "Treatment deleted successfully."
}
```

### Endpoint 20 - Register User

**URL:** `/api/Users/Register`  
**Method:** POST  
**Description:** Registers a new user with the "User" role.  
**Request Body:**

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "userName": "johndoe",
  "email": "john@example.com",
  "phoneNumber": "0701234567",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

**Response:**

```json
{
  "id": "user-id",
  "userName": "johndoe",
  "email": "john@example.com",
  "phoneNumber": "0701234567",
  "role": "User"
}
```

### Endpoint 21 - Get User by ID

**URL:** `/api/Users/{id}`  
**Method:** GET  
**Description:** Retrieves details of a specific user.  
**Request Parameters:**

* `id` (string) – The unique identifier of the user

**Response:**

```json
{
  "id": "user-id",
  "userName": "johndoe",
  "email": "john@example.com",
  "phoneNumber": "0701234567",
  "role": "User"
}
```

### Endpoint 22 - Update User

**URL:** `/api/Users/{id}`  
**Method:** PUT  
**Description:** Updates a user's information.  
**Request Parameters:**

* `id` (string) – The unique identifier of the user

**Request Body:**

```json
{
  "id": "user-id",
  "firstName": "John",
  "lastName": "Doe",
  "userName": "johndoe",
  "email": "john@example.com",
  "phoneNumber": "0701234567"
}
```

**Response:**

```json
{
  "message": "User updated successfully."
}
```

### Endpoint 23 - Get All Users

**URL:** `/api/Users`  
**Method:** GET  
**Description:** Retrieves a list of all registered users.  
**Request Parameters:** _None_

**Response:**

```json
[
  {
    "id": "user-id-1",
    "userName": "johndoe",
    "email": "john@example.com",
    "phoneNumber": "0701234567",
    "role": "User"
  },
  {
    "id": "user-id-2",
    "userName": "janedoe",
    "email": "jane@example.com",
    "phoneNumber": "0707654321",
    "role": "Hairdresser"
  }
]
```
