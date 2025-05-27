
# Unit Test Strategy - Hairdressser API
## 1. Purpose
The focus is on validating repository logic and controller behavior through automated unit tests.

---
## 2. Scope
- **Repository tests**: CRUD Operations
- **Controller tests**: HTTP logic, Service logic, status codes and error handlin
---
## 4. Test Structure

```
/Projekt
  /HairdresserUnitTests
   - HairdresserControllerTests.cs
   - BookingControllerTests.cs
   - TreatmentControllerTests.cs
   - UserControllerTests.cs
   - BookingRepositoryTests.cs
   - TreatmentRepositoryTests.cs
```
---
## 3. Naming Conventions

- **Test project**: `HairdresserUnitTests`
- **Test methods**: `MethodName_Condition_ExpectedResult`

```csharp
[TestMethod]
public void GetUser_ValidId_Returns200Ok()
```
---
## 4. Summary

- Service layer is not tested separately.
- Coverage is focused on repositories and controllers.
