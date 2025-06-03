# Shetlandsponnyerna

## API Endpoints

### BookingController
| Method | Endpoint | Authentication |
|--------|----------|----------------|
| GET    | `/User/Booking` | ✅ Authenticated |
| POST   | `/User/Booking` | ✅ Authenticated |
| PUT    | `/User/Booking` | ✅ Authenticated |
| DELETE | `/User/Booking` | ✅ Authenticated |
| GET    | `/Booking` | ❌ Not Authenticated |

### TreatmentController
| Method | Endpoint | Authentication |
|--------|----------|----------------|
| GET    | `/Treatments` | ❌ Not Authenticated |
| POST   | `/Treatments` | ✅ Authenticated |
| PUT    | `/Treatments/Id` | ✅ Authenticated |
| DELETE | `/Treatments/Id` | ✅ Authenticated |

### HairdresserController
| Method | Endpoint | Authentication |
|--------|----------|----------------|
| GET    | `/Hairdresser/Id` | ❌ Not Authenticated |
| GET    | `/Hairdresser` | ❌ Not Authenticated |
| POST   | `/Hairdresser` | ✅ Authenticated |
| PUT    | `/Hairdresser/Id` | ✅ Authenticated |
| DELETE | `/Hairdresser/Id` | ✅ Authenticated |


## Project Description and Architecture Overview

Provide a brief introduction to your project, including its main purpose, target audience, and core functionality. Include a high-level architecture diagram if possible, highlighting the key components and their interactions.

### Key Features

* Feature 1
* Feature 2
* Feature 3

### Architecture Overview

Explain the system architecture, including:

* Microservices or monolith structure
* Database and data flow
* Key technologies and frameworks
* Deployment environment

---


## API Endpoints Documentation

Describe the available API endpoints, including request and response examples. Use the format below for consistency:

### Endpoint 1 - Get All Bookings Overview

**URL:** `/api/Admin/bookings-overview`  
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

**URL:** `/api/Admin/add-hairdresser`  
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

**URL:** `/api/Authentication/AuthLogin`  
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

**URL:** `/api/Bookings/Available-times`  
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

**URL:** `/api/Bookings/BookingsById`  
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

**URL:** `/api/Bookings/Book Appointment`  
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

**URL:** `/api/Bookings/Reschedule`  
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

**URL:** `/api/Hairdresser/schedule`  
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

**URL:** `/api/Hairdresser/monthly-schedule`  
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

**URL:** `/api/Hairdresser/{id}`  
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

**URL:** `/api/Users/registerUser`  
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

---

## Test Strategy and Results

Outline your test strategy, including the types of tests performed (e.g., unit, integration, E2E) and tools used. Include a summary of test coverage and key results.

### Test Strategy

* Unit Tests: Briefly describe the approach and tools (e.g., Jest, Pytest).
* Integration Tests: Describe how services are tested together.
* End-to-End (E2E) Tests: Detail the approach for full workflow testing.

### Test Results

Include a summary of test results, including coverage reports or key findings.
