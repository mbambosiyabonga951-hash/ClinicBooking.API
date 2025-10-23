# Clinic Booking

Clean Architecture starter with **.NET 8**, **ASP.NET Core (controllers)**, **EF Core + Dapper**, and **NLog**.

## Projects
- **ClinicBooking.Api** — Web API (Swagger enabled)
- **ClinicBooking.Application** — DTOs, interfaces, and services
- **ClinicBooking.Domain** — Entities
- **ClinicBooking.Infrastructure** — EF Core DbContext + Dapper repositories + DI
- **ClinicBooking.Tests** — xUnit tests

## Quick Start
1. Open `ClinicBooking.sln` in Visual Studio 2022+
2. Ensure SQL Server is available and update `src/ClinicBooking.Api/appsettings.json` connection string.
3. Create tables:
   ```sql
   CREATE TABLE dbo.Patients (Id BIGINT IDENTITY(1,1) PRIMARY KEY, FirstName NVARCHAR(100), LastName NVARCHAR(100), Email NVARCHAR(255));
   CREATE TABLE dbo.Clinics (Id BIGINT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(200), Address NVARCHAR(255));
   CREATE TABLE dbo.Appointments (Id BIGINT IDENTITY(1,1) PRIMARY KEY, ClinicId BIGINT, PatientId BIGINT, Date DATE, StartTime TIME, EndTime TIME);
   CREATE INDEX IX_Appointments_ClinicId_Date ON dbo.Appointments(ClinicId, Date);
   ```
4. Run the API project. Swagger should appear at `/swagger`.

## Sample Endpoints
- `GET /api/patients`
- `POST /api/patients` (CreatePatientRequest)
- `GET /api/clinics`
- `GET /api/appointments/by-clinic/{clinicId}/date/{date}` (date format: `2025-10-22`)
- `POST /api/appointments` (CreateAppointmentRequest)

## Notes
- Logging via NLog (`NLog.config`).
- EF Core configured in `Program.cs` and DbContext in Infrastructure.
- Dapper used via repositories for read/write examples.
