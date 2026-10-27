# PilotAssess

A web app for flight instructors to run scenario-based simulator assessments. Instructors build a scenario guide (SEG), grade each pilot on the behaviours they observe as the scenario plays out, and export a filled-in report card.

**Stack:** Angular 19 · PrimeNG · Tailwind CSS · ASP.NET Core 8 Web API · Entity Framework Core 9 · SQLite

## Features

- **Scenario builder:** put event triggers (malfunctions, procedures and other events) in order, and attach the behaviours an instructor should look for at each step. Existing SEGs can be edited or deleted.
- **Live grading:** run a SEG against a crew. Each behaviour is graded −1, 0 or +1, separately for the flying pilot and the non-flying pilot.
- **Instructor critique:** give a score and a comment per category alongside the behaviour grades.
- **Records:** browse past SEGs and submitted assessments.
- **Report cards:** from the records page, download a pilot's report card. The API fills a Word (`.docx`) marking sheet template from the stored grades.
- **Bulk import:** load the catalogue of behaviours and triggers through bulk-insert endpoints (EFCore.BulkExtensions).
- **Instructor sign-in:** every API endpoint requires a signed-in instructor. Passwords are hashed with ASP.NET Core Identity's `PasswordHasher` (PBKDF2). Sessions use an HTTP-only, `SameSite=Strict` cookie, and login attempts are limited to 5 per minute per IP address.

## Architecture

```
Angular SPA (PrimeNG + Tailwind)
        │  REST / JSON
        ▼
ASP.NET Core Web API
  Controllers ──► Services (interface-based, DI) ──► EF Core ──► SQLite
        │
        └─ DocX: fills the report card template
```

In production, the built Angular app is served from the API's `wwwroot`. Unknown routes fall back to `index.html`, so the whole tool ships as a single deployable that also runs offline.

### Core data model

![Core data model](myFlightPerformanceBackend/Human%20Performance%20Database%20Reviewed.png)

A SEG holds an ordered list of triggers. Each trigger links to the behaviours that should be observed. An observed event is one run of a SEG for a pair of pilots, and it records one grade per behaviour per pilot.

## Run it locally

**Requirements:** .NET 8 SDK and Node.js 18 or newer.

The API applies database migrations when it starts. On the first run, set the first instructor account through environment variables. Once that account exists, the variables are ignored.

```bash
# API: http://localhost:5000 (Swagger UI at /swagger in Development)
cd myFlightPerformanceBackend
export InitialInstructor__Username=admin
export InitialInstructor__Password='choose-a-long-password'
dotnet run

# Frontend: http://localhost:4200 (in a second terminal)
# The dev server proxies /api to the API, so the session cookie stays same-origin.
cd myFlightPerformanceFrontend
npm install
npm start
```

On Windows PowerShell, set the variables with `$env:InitialInstructor__Username = "admin"` instead of `export`. Signed-in instructors can add more accounts with `POST /api/auth/instructors`.

A packaged build of an earlier version, from before sign-in was added, is in [`build/`](build).

## Project layout

```
myFlightPerformanceBackend/
  Controllers/      REST endpoints (auth, SEGs, assessments, report cards, bulk import)
  Services/         Business logic behind interfaces
  Models/           EF Core entities and request DTOs
  Migrations/       EF Core schema history
  Forms/            Report card .docx template
myFlightPerformanceFrontend/
  src/app/pages/    Login, home, SEG builder, grading, records and report card download
  src/app/services/ HTTP clients for the API, plus the auth guard and interceptor
  src/app/models/   TypeScript types matching the API
```
