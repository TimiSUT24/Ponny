# Unit Test Catalog

---
## BookingControllerTests

Tests for the `BookingsController`, which handles booking, rebooking, and handling booking conflicts. These tests mock `IBookingService` and simulate booking scenarios for authenticated users.

| **Test Method**                                       | **Purpose**                                                                                     |
|-------------------------------------------------------|-------------------------------------------------------------------------------------------------|
| `BookAppointment_ReturnsCreatedWithCorrectBookingData` | Ensures that a successful booking request returns `201 Created` with correct booking details.   |
| `Book_NonExisting_Treatment_Returns404`              | Validates that trying to book with a non-existent treatment returns `404 Not Found`.            |
| `Book_Occupied_Time_ReturnsConflict`                 | Simulates a conflict due to a time slot already being booked; expects a `409 Conflict` result.  |
| `Book_InvalidTime_ReturnsConflict`                   | Verifies that invalid booking times result in a `409 Conflict` from the service.                |
| `Rebook_ValidRequest_ReturnsOkWithUpdatedBooking`     | Tests that a valid rebooking request returns `200 OK` with updated booking information.         |

---
## BookingRepositoryTests

Tests for the `BookingRepository`, which is responsible for managing data access related to `Booking` entities using Entity Framework Core.

| **Test Method**                        | **Purpose**                                                                                      |
|----------------------------------------|--------------------------------------------------------------------------------------------------|
| `Add_ShouldAddBookingSuccessfully`     | Verifies that a new booking is added correctly to the database and can be retrieved afterward.   |
| `Delete_ShouldDeleteBookingSuccessfully` | Ensures that a booking is successfully deleted and no longer exists in the database.             |

---
## HairdresserControllerTest

Tests for the `HairdresserController`, which handles retrieving hairdresser-related data from the database.

| **Test Method**                             | **Purpose**                                                                                   |
|---------------------------------------------|-----------------------------------------------------------------------------------------------|
| `GetAll_ShouldReturnAllHairdressers_Users`  | Verifies that all hairdressers are returned when present in the database.                    |
| `GetAll_ShouldReturnEmptyList_WhenNoHairdressers` | Ensures an empty list is returned when no hairdressers exist in the database.                |
| `GetHairdresserById_ShudlReturnHairdresserIdWhenFound` | Checks that a specific hairdresser is returned correctly when found by ID.               |

---
## TreatmentControllerTests

Unit tests for the `TreatmentController`, which manages creation, retrieval, update, and deletion of treatments.

| **Test Method**                       | **Purpose**                                                                 |
|--------------------------------------|------------------------------------------------------------------------------|
| `GetAll_ReturnsAllTreatments`        | Verifies that the controller returns all treatments correctly.              |
| `Create_ValidTreatment_ReturnsCreated` | Confirms that creating a valid treatment returns a `201 Created` result.    |
| `Update_NonExistingId_ReturnsNotFound` | Checks that updating a treatment with a non-existent ID returns `404`.      |
| `Delete_ValidId_DeletesTreatment`    | Ensures a treatment with a valid ID is deleted and returns `204`.           |
| `Delete_InvalidId_ReturnsNotFound`   | Validates that deleting a non-existent treatment ID returns `404 Not Found`.|

---
## TreatmentRepositoryTests

Unit tests for the `TreatmentRepository`, which handles CRUD operations for treatments in the database.

| **Test Method**                                               | **Purpose**                                                                 |
|---------------------------------------------------------------|------------------------------------------------------------------------------|
| `Add_ShoudAddTreatmentSuccessfully`                           | Verifies that a treatment is added and retrievable from the repository.     |
| `Delete_ShoudDeleteTreatmentSuccessfully`                     | Ensures a treatment is deleted and no longer exists in the repository.      |
| `GetAllAsync_ShouldReturnAllTreatments`                       | Checks that all added treatments are retrieved correctly.                    |
| `GetByIdAsync_ShouldReturnCorrectTreatment`                   | Validates fetching treatment by ID returns correct entity or null.          |
| `UpdateAsync_ShouldUpdateTreatmentSuccessfully`               | Confirms updates to a treatment's fields are saved and reflected properly.  |
| `AddAsync_ShouldThrowException_WhenEntityIsNull`              | Ensures `ArgumentNullException` is thrown when null is passed to AddAsync.  |
| `DeleteAsync_ShouldThrowException_WhenEntityIsNull`           | Ensures `ArgumentNullException` is thrown when null is passed to DeleteAsync. |

---
## UsersControllerTests

Unit tests for the `UsersController`, focusing on user registration, retrieval, and updates. And Admin for tasks like retrieving booking overviews and registering hairdressers. Uses mocked `IAdminService` and `UserManager`, and simulates authenticated admin access.

| **Test Method**                                 | **Purpose**                                                                 |
|-------------------------------------------------|------------------------------------------------------------------------------|
| `Register_CreatesUser_ReturnsCreatedAtAction`   | Ensures user registration succeeds and returns a `CreatedAtActionResult`.   |
| `GetById_UserExists_ReturnsUser`                | Validates that a user can be retrieved by ID if they exist.                 |
| `Update_UserExists_UpdatesInfo`                 | Confirms that updating an existing user's information works as expected.    |
| `Register_AddsUserRole_User`                    | Checks that the registered user is assigned the `"User"` role.              |
| `GetAllBookingsOverview_ReturnsOkWithData`                | Verifies that the controller returns `200 OK` with booking data from the service.             |
| `CreateHairdresser_ValidInput_ReturnsOkWithUserDto`       | Ensures valid input creates a new user and returns a `UserDto`.                               |
| `CreateHairdresser_InvalidModelState_ReturnsBadRequest`   | Confirms `400 BadRequest` is returned for invalid or incomplete model input.                  |
| `CreateHairdresser_CreateFails_ReturnsBadRequestWithErrors` | Simulates identity creation failure and verifies error response is correctly handled.        |
| `CreateHairdresser_AddToRole_CalledWithCorrectRole`       | Asserts that the new user is correctly added to the `"Hairdresser"` role.                    |


