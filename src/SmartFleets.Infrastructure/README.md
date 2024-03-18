# SmartFleets.Infrastructure

This project provides the implementation for infrastructure concerns such as data persistence, message bus integration, external service clients, and other cross-cutting concerns.
Responsibilities: Implementing repositories for data access, configuring message bus communication (e.g., RabbitMQ), and providing implementations for external service interfaces.

### Guides

- [Entity Framework](#entity-framework)

## Entity Framework

Table of content
- [Add migration](#add-migration)
- [Update migration](#update-database)
- [Remove migration](#remove-migration)

#### Add migration
In order to add a new migration to the project, 
- Select 'SmartFleets.Api' as startup project
- Open Package Manager Console
- Select 'SmartFleets.Api' as 'Default project'
- Put the command below
  ```powershell
  Add-Migration -Name '<your_migration_name>' -output 'Data/Migrations'  
  ```
- Press enter

ℹ️ Note: The migration name should be stylized as `snake_case`.

#### Update database
In order to update the database, 
- Select 'SmartFleets.Api' as startup project
- Open Package Manager Console
- Select 'SmartFleets.Api' as 'Default project'
- Add an connection string for 'SmartFleets.Api' at 'appsettings.json' containing your database at 'Data/Migrations'.
- Put the command below
  ```powershell
  Update-Database  
  ```
- Press enter

#### Remove migration
In order to remove a migration to the project, 
- Select 'SmartFleets.Api' as startup project
- Open Package Manager Console
- Select 'SmartFleets.Api' as 'Default project'
- Add an connection string for 'SmartFleets.Api' at 'appsettings.json' containing your database at 'Data/Migrations'.
- Put the command below
  ```powershell
  Remove-Migration  
  ```
- Press enter
