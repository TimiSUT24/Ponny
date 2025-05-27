# Unit Test Catalog

## AdminControllerTests

Tests the `AdminController` endpoints for tasks like retrieving booking overviews and registering hairdressers. Uses mocked `IAdminService` and `UserManager`, and simulates authenticated admin access.

| **Test Method**                                           | **Purpose**                                                                                   |
|-----------------------------------------------------------|-----------------------------------------------------------------------------------------------|
| `GetAllBookingsOverview_ReturnsOkWithData`                | Verifies that the controller returns `200 OK` with booking data from the service.             |
| `CreateHairdresser_ValidInput_ReturnsOkWithUserDto`       | Ensures valid input creates a new user and returns a `UserDto`.                               |
| `CreateHairdresser_InvalidModelState_ReturnsBadRequest`   | Confirms `400 BadRequest` is returned for invalid or incomplete model input.                  |
| `CreateHairdresser_CreateFails_ReturnsBadRequestWithErrors` | Simulates identity creation failure and verifies error response is correctly handled.        |
| `CreateHairdresser_AddToRole_CalledWithCorrectRole`       | Asserts that the new user is correctly added to the `"Hairdresser"` role.                    |

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



