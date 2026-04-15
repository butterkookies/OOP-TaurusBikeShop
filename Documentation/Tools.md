# Taurus Bike Shop — Complete Ecosystem Download List (TXT Format)
# Revised for Website instead of Mobile App (accessible on any device)

1. Microsoft Visual Studio
   Purpose: Main IDE for building all C# projects (ASP.NET Core backend, WPF Admin App, Blazor Web Frontend)
   Required Config: Workload: ASP.NET and web development; Workload: .NET desktop development
   Source: https://visualstudio.microsoft.com/
   Notes: Essential for compiling and managing all C# projects in one environment

2. .NET SDK (Latest LTS version)
   Purpose: Runtime and compiler for ASP.NET Core, WPF, and Blazor Web projects
   Required Config: Verify installation via 'dotnet --version'
   Source: https://dotnet.microsoft.com/download
   Notes: Usually installed automatically with Visual Studio; ensures all projects build correctly

3. Microsoft SQL Server (Express Edition)
   Purpose: Relational database for storing products, sales, customers, users, inventory
   Required Config: Install default instance; Enable TCP/IP for remote connections if needed
   Source: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   Notes: Centralized database for both Admin and Customer Web clients

4. SQL Server Management Studio (SSMS)
   Purpose: GUI tool for managing SQL Server (queries, tables, relationships, backups)
   Required Config: Connect to local SQL Server instance; Optional: Azure SQL connection for cloud deployment
   Source: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
   Notes: Used for database schema design, testing queries, and admin tasks

5. Git
   Purpose: Local version control system for tracking changes, branching, collaboration
   Required Config: Configure user.name and user.email; Optional: SSH key for GitHub authentication
   Source: https://git-scm.com/
   Notes: Critical for maintaining project history and enabling multiple developers to work safely

6. GitHub (Account)
   Purpose: Remote repository hosting for collaboration, backup, pull requests
   Required Config: Create repository for project; Use GitHub Desktop or CLI for push/pull
   Source: https://github.com/
   Notes: Centralized remote repository for all source code

7. Postman
   Purpose: API testing tool to send HTTP requests, debug authentication, inspect JSON responses
   Required Config: Install desktop or use web version; Configure environment variables for API URLs
   Source: https://www.postman.com/downloads/
   Notes: Ensures backend APIs work correctly before connecting web or desktop clients

8. Docker Desktop (Optional)
   Purpose: Containerize backend for easier deployment and environment consistency
   Required Config: Enable WSL 2 backend (Windows) or Docker Engine (Mac/Linux); Optional: Docker Compose for multi-container setup
   Source: https://www.docker.com/products/docker-desktop
   Notes: Professional workflow for deployment, testing, and scalability

9. Figma
   Purpose: UI wireframes and design; plan website layouts and Admin interface
   Required Config: Create account; Optional: install desktop app for offline design
   Source: https://www.figma.com/
   Notes: Pre-coding visual planning for both customer web interface and admin interface

10. Draw.io
    Purpose: Diagramming tool for ERD, system architecture, flowcharts
    Required Config: Web-based; no install required; Optional desktop app available
    Source: https://app.diagrams.net/
    Notes: Used to plan database schema, architecture, and system workflows visually

11. Azure Account
    Purpose: Cloud hosting for ASP.NET Core backend API and SQL database; production deployment
    Required Config: Create Azure App Service for ASP.NET Core; Configure Azure SQL Database; Setup SSL / HTTPS certificates
    Source: https://azure.microsoft.com/
    Notes: Necessary for production deployment of web-accessible APIs and centralized database

# Minimum Required to Start:
# Visual Studio, .NET SDK, SQL Server, SSMS, Git, GitHub Account, Postman
