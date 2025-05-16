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

### Endpoint 1 - Description

**URL:** `/api/v1/example`
**Method:** GET
**Description:** Briefly describe the purpose of this endpoint.
**Request Parameters:**

* `param1` (type) - description
* `param2` (type) - description

**Response:**

```json
{
  "key": "value"
}
```

### Endpoint 2 - Description

**URL:** `/api/v1/example`
**Method:** POST
**Description:** Briefly describe the purpose of this endpoint.
**Request Body:**

```json
{
  "param1": "value",
  "param2": "value"
}
```

**Response:**

```json
{
  "message": "Success"
}
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
