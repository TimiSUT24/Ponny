# Unit Test Catalog

---
## BookingServiceTests

Tests for the `BookingServiceTests`, which handles logic for booking, rebooking, cancel booking and get booking 

| **Test Method**                                       | **Purpose**                                                                                     |
|-------------------------------------------------------|-------------------------------------------------------------------------------------------------|
| `BookAppointment_ShouldReturnBooking_WhenBookingIsSuccessful` | Ensures that when a booking is booked that is returns a booking   |
| `BookAppointment_ShouldThrowKeyNotFoundException_WhenHairdresserNotFound`| Ensures that when booking an appointment with no hairdresser that it throws exception Not Found. |
| `BookAppointment_ShouldThrowKeyNotFoundException_WhenTreatmentNotFound` | Ensures that when booking an appointment with no treatment that it throws exception Not Found.   |
| `CancelBooking_ShouldReturnCancelledBooking_WhenCancellationIsSuccessful` Ensures when a booking is cancelled it returns a cancelled booking |
| `CancelBooking_ShouldReturnKeyNotFound_WhenBookingIsNotFoundForThatCustomer`| Ensures when booking doesnt exist for a customer it throw exception Not Found |
| `GetAllAvailableTimes_ShouldReturnCorrectTimeSlots`| Ensures that it returns a list of timeslots for a specific hairdresser/treatment for specific day |
| `GetAllAvailableTimes_ShouldThrowArgumentException_WhenDayIsInvalid`| Ensures that it throws argument exception when date is invalid|
| `GetAllAvailableTimes_ShouldThrowKeyNotFoundException_WhenHairdresserIsNotFound`| Ensures it throws Not Found when hairdresser doesnt exist when getting timeslots for a hairdresser |
| `GetAllAvailableTimes_ShouldThrowKeyNowFoundException_WhenTreatmentIsNotFound`| Ensures it throws Not Found when treatment doesnt exist when getting timeslots for a hairdresser |
| `GetBookingByIdAsync_ShouldReturnRightCustomerBookingWithDetails`| Ensures that it returns booking details for a specific customer only|
| `GetBookingByIdAsync_ShouldThrowKeyNotFoundException_WhenBookingIsNotFound`| Ensures that it throws Not Found when booking doesnt exist |
| `RebookBooking_ShouldReturnBooking_WhenRebookingIsSuccessful`| Ensures that it returns a updated booking when Rescheduled |
| `RebookBooking_ShouldThrowArgumentException_WhenRebookingIsBookedInThePast`| Ensures that it throws argument exception when a booked in the past |
| `RebookBooking_ShouldThrowKeyNotFoundException_WhenBookingIsNotFound`| Ensures that it throws Not Found when booking doesnt exist |

---
## BookingRepositoryTests

Tests for the `BookingRepository`, which is responsible for managing data access related to `Booking` entities using Entity Framework Core.

| **Test Method**                        | **Purpose**                                                                                      |
|----------------------------------------|--------------------------------------------------------------------------------------------------|
| `Add_ShouldAddBookingSuccessfully`     | Verifies that a new booking is added correctly to the database and can be retrieved afterward.   |
| `Delete_ShouldDeleteBookingSuccessfully` | Ensures that a booking is successfully deleted and no longer exists in the database.           |
| `AnyAsync_ShouldReturnTrueIfAnyBookingExistForHairdresser` | Ensures that a booking is found for a specific hairdresser                   |
| `FindAsync_ShouldReturnBookingsByPredicateSuccessfully` | Should return a booking if bookingId exist      |
| `GetAllAsync_ShouldReturnAllBookingsSuccessfully` | Ensures that it returns all bookings that exist        |
| `GetByIdAsync_ShouldReturnBookingByIdSuccessfully` | Ensures that it returns a booking by id         |
| `GetByIdWithDetailsAsync_ShouldReturnBookingForTheRightCustomer` | Ensures that it returns a booking for a specific customer only         |
| `GetMonthlyScheduleWithDetailsAsync_ShouldReturnBookingsForTheRightHairdresser` | Ensures that it returns bookings for specific hairdresser based on month          |
| `GetWeekScheduleWithDetailsAsync_ShouldReturnBookingsForTheRightHairdresser` |  Ensures that it returns bookings for specific hairdresser based on week        |
| `SaveChangesAsync_ShouldSaveChangesSuccessfully` | Ensures when a booking is updated that it is saved correctly        |
| `UpdateAsync_ShouldUpdateBookingSuccessfully` | Ensures when a booking is updated that is got correctly updated     |

## TreatmentServiceTests

Unit tests for the `TreatmentServiceTests`, 

| **Test Method**                       | **Purpose**                                                                 |
|--------------------------------------|------------------------------------------------------------------------------|
| `GetAllAsync_ShouldReturnAllTreatments`  | Verify that the service correctly retrieves all treatments.       |
| `GetByIdAsync_ShouldReturnTreatment_WhenExists` | Ensure that a treatment is returned when it exists.   |
| `GetByIdAsync_ShouldReturnNull_WhenNotExists` | Ensure that null is returned when a treatment with the given ID does not exist.|
| `GetByIdAsync_ShouldReturnNull_WhenIdIsZeroOrless`    | Ensure that invalid IDs (≤ 0) result in a null response.         |
| `CreateAsync_ShouldAddTreatment`   | Test that a treatment is added successfully.|
| `UpdateAsync_ShouldReturnTrue_WhenTreatmentExists`   | Check that an update succeeds if the treatment exists.|
| `UpdateAsync_ShouldReturnFalse_WhenTreatmentDoesNotExist`   |Ensure no update is done when treatment does not exist.|
| `UpdateAsync_ShouldReturnFalse_WhenIdIsZeroOrLess`   | Invalid IDs (≤ 0) should not trigger an update.|
| `DeleteAsync_ShouldReturnTrue_WhenTreatmentHaveBeenRemoved`   | Validates that a treatment can be deleted successfully.|
| `DeleteAsync_ShouldReturnFalse_WhenTreatmentDoesNotExist`   | Deletion should fail for non-existing treatment.|
| `DeleteAsync_ShouldReturnFalse_WhenIdIsZeroOrLess`   | No deletion should occur for invalid IDs.|

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
## UserRepositoryTests

Unit tests for the `UserRepositoryTests`, 

| **Test Method**                                 | **Purpose**                                                                 |
|-------------------------------------------------|------------------------------------------------------------------------------|
| `AddAsync_ShouldAddUser`   | Verifies that the AddAsync method correctly adds a new user to the database.   |
| `DeleteAsync_ShouldRemoveUser`                | Ensures that the DeleteAsync method successfully removes an existing user from the database.                 |
| `GetByIdAsync_ShouldReturnUser`                 | Tests that a user can be retrieved by their unique identifier using the GetByIdAsync method.    |
| `FindAsync_ShouldReturnMatchingUsers`                    | Validates that the FindAsync method returns users matching a given predicate.              |
| `RegisterUserAsync_InvalidPassword_ShouldNotCreate`                | Ensures that registration fails when an invalid password is provided.             |
| `RegisterUserAsync_InvalidEmail_ShouldNotCreate`       | Verifies that registration fails when an invalid email address is used.                               |
| `RegisterUserAsync_ShouldReturn_UserDto`   | Confirms that registration works correctly when valid user data is provided.                  |
| `RegisterUserAsync_DuplicateUsername_ShouldNotCreate` | Ensures that the system prevents multiple registrations with the same username.        |
| `GetHairdressersWithBookings_ShouldReturn_Hairdressers`       | Tests that the repository correctly identifies hairdressers who have active bookings.                    |
---

## UserServiceTests

Unit tests for the `UserServiceTests`, 

| **Test Method**                                 | **Purpose**                                                                 |
|-------------------------------------------------|------------------------------------------------------------------------------|
| `GetAllHairdressersAsync_ShouldReturnHairdressers`   | Tests that the service correctly returns a list of hairdressers.   |
| `GetAllHairdressersAsync_ShouldReturnEmptyList_WhenNoUsersExist`   | Ensures that it returns empty list when no hairdresserexist  |
| `GetWeekScheduleAsync_ShouldReturnBookingsForWeek`   | Verifies that a weekly schedule of bookings is returned for a hairdresser.   |
| `GetWeekScheduleAsync_GetEmptyHairdresserId_ReturnsEmptyList`   | Ensures that passing an empty or null hairdresser ID throws an exception.   |
| `GetWeekScheduleAsync_GetDateInPast_ReturnsEmptyList`   | Ensures that if a past date is given, the method returns an empty list.  |
| `GetWeekScheduleAsync_ShouldHandleEmptyBookings`   | Ensures that there are no booking for a specific week it can handle an empty list   |
| `GetMonthlyScheduleAsync_ShouldReturnBookingsForMonth`   | Verifies that bookings are returned for a specific month and year.   |
| `GetMonthlyScheduleAsync_GetEmptyHairdresserId_ReturnsEmptyList`   | Verifies that an invalid or missing hairdresser ID results in an exception.   |
| `GetMonthlyScheduleAsync_InvalidDateFilter_ReturnsEmptyList`   | Ensures that invalid month or year values return no bookings.   |
| `GetBookingDetailsAsync_ShouldReturnBookingDetails`   | Confirms that booking details are returned when given a valid booking ID.   |
| `GetBookingDetailsAsync_BookingNotFound_ReturnsNull`   | Verifies behavior when a booking with the given ID does not exist.   |
| `UpdateHairdresserAsync_ShouldUpdateHairdresser`   |Tests that a hairdresser's information is updated successfully.  |
| `UpdateHairdresserAsync_HairdresserNotFound_ReturnsNull`   | Ensures that trying to update a non-existent hairdresser returns null.   |
| `UpdateHairdresserAsync_HairdresserNotFound_ReturnsNull`   | Ensures that trying to update a non-existent hairdresser returns null.   |
| `GetHairdresserWithId_ShouldReturnNull_WhenNotFound`   | Ensures that it throws Not Found exception when hairdresser doesnt exist  |
| `GetHairdresserWithId_ShouldReturnHairdresser`   | Ensures that it returns a hairdresser |
| `GetHairdresserWithId_EmptyId_throwException`   | Ensures that it throws Argument exception when hairdresserId is null or empty  |

