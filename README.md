[README.md](https://github.com/user-attachments/files/33581040/README.md)
# API_proj_2

Small race-event management ASP.NET Core Web API built with .NET 8 and EF Core.

## Overview

This repository implements a simple event management API for creating events, categories, routes, enrolments and results. It uses Entity Framework Core for data access and SQL Server as the intended database provider.

## Prerequisites

- .NET 8 SDK
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code

## Configuration

1. Update `appsettings.json` `ConnectionStrings:DefaultConnection` or use user secrets for local development.
2. To use user secrets in the project directory:

## Database migrations

Apply the existing initial migration to create the schema:

- CLI:
- Visual Studio Package Manager Console:

Generate a new migration after model changes:
	
Get an SQL script for CI or review:

## Run the API

- From command line:
- From Visual Studio: set `API_proj_2` as the startup project and run (F5).

## API endpoints (high level)

- `POST /api/auth/register` � register user
- `POST /api/auth/login` � get JWT
- `GET /api/events` � list events
- `POST /api/events` � create event (organiser role)
- `GET /api/categories` � list categories
- `POST /api/enrolments` � enrol user into event category
- `POST /api/results` � create result for enrolment

Refer to controllers in `API_proj_2/Controllers` for full routes and request/response schemas.

## Testing

Unit tests are located in `TestProject1`. Run tests with:

## Contributing

Follow project coding styles and the `.editorconfig`/`CONTRIBUTING.md` guidelines. Open issues or PRs against `master` and include tests for new behavior. Keep changes small and focused.

## Troubleshooting

- Migration errors: ensure the connection string points to a SQL Server instance and the database user has permission to create databases.
- Unique index conflicts (e.g., email or bib numbers): clear test data or update the seeded values before applying migrations.

## License

This project is provided as-is. Check the repository for licensing details.	
