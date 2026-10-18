# Factory Management — Windows Forms Prototype

A legacy Windows Forms project for exploring factory, warehouse, product and customer workflows. The source is written in C# and targets **.NET Framework 4.7.2**, with SQL Server as its data store.

## Current capabilities in the source

- Customer and manufacturer login forms
- Product browsing and customer order entry
- Manufacturer workflow screens
- Warehouse selection and inventory updates
- Basic employee and factory messages

These are **source-code features**, not a claim that the entire application has been rebuilt, deployed or independently tested.

## Requirements

- Windows with Visual Studio and the .NET desktop development workload
- .NET Framework 4.7.2 targeting/developer pack
- SQL Server or SQL Server Express
- A compatible `FactoryManagement` database

**Database limitation:** this repository does not currently include a verified database creation script, migrations, or seed data. Building the solution does not mean its database-dependent screens are functional. A compatible schema must be sourced and reviewed before functional testing.

## Database connection

Connection strings are read from the `FactoryManagement` entry in `App.config`. The included default assumes a local SQL Server Express instance.

For another environment, set the `FACTORY_MANAGEMENT_CONNECTION_STRING` environment variable before starting the app, or edit the `App.config` connection string in your local copy.

Example (PowerShell; Windows authentication):

```powershell
$env:FACTORY_MANAGEMENT_CONNECTION_STRING = 'Data Source=.\SQLEXPRESS;Initial Catalog=FactoryManagement;Integrated Security=True'
```

No credentials should be committed to Git. For real data, apply least-privilege SQL access and use a protected configuration mechanism.

## Build

1. Open `VP_Midterm_Project.sln` in Visual Studio.
2. Restore required framework components.
3. Build the solution for `Release | Any CPU`.
4. Review `docs/TESTING.md` before attempting functional use.

A Windows CI build checks **compilation only**. It does not provision the SQL Server database or verify application behavior.

## Known limitations

- The database schema and repeatable sample dataset are not included.
- Authentication checks passwords against database fields in their current legacy form; there is no demonstrated modern password-hashing design. **Do not deploy this prototype as a production authentication service.**
- Error handling, validation, transaction behavior, and data access require further review.
- No real-world reliability, security, performance or business-impact metrics have been established.

## Project provenance

This repository preserves its original commit history, which includes commits attributed to `Haroon-Khan11`. Repository ownership and transfer to the current GitHub account do not imply sole authorship of the original source code. Contributions should be credited according to verifiable records.

## Development status

**Legacy prototype / refactoring in progress** — not production-ready.
