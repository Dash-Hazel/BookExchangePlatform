# 🚀 POV Platform

> A personal content management platform where users can catalog and manage their own collections of books and movies.

![.NET Version](https://img.shields.io/badge/.NET-10.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Why the Name Changed](#why-the-name-changed)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Features](#features)
- [Usage](#usage)
- [Getting Started](#getting-started)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

# 📖 About the Project

**POV Platform** is a web application built with ASP.NET Core MVC that allows users to manage their personal collection of books and movies. Users can register, log in, and add, edit, view, and delete their own books and movies.

The project was developed as the final assignment for the **ASP.NET Fundamentals course at SoftUni**.

---

# 🔄 Why the Name Changed

You might notice that the project is called **POV Platform**, but the code files still reference `BookExchangePlatform`. Here's why:

The project was **originally envisioned as a book exchange platform** — hence the original namespace `BookExchangePlatform`. However, during development, the idea evolved into a **personal content manager** (POV — Point of View) to focus on user‑owned collections rather than exchanges between users. This shift happened because implementing secure user‑to‑user exchange features was beyond the scope of the fundamentals course.

The core functionality remains intact, and the namespace was kept to avoid breaking the project structure. **The app works perfectly** — just think of it as a personal catalog for your books and movies.

---

# 🛠️ Technologies Used

- ASP.NET Core MVC 10.0
- Entity Framework Core 10.0.3
- SQL Server 2022 (Docker)
- ASP.NET Core Identity 10.0.3
- Bootstrap 5.3
- Razor Views

---

# ✅ Prerequisites

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

---

#🗄️ Database Setup

Connection string (`appsettings.json`):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=PovPlatform;User Id=sa;Password=Ldeyvis123;TrustServerCertificate=True;"
}
```
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```


---
#⚙️ Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=PovPlatform;User Id=sa;Password=Ldeyvis123;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```
⚠️ Never commit real passwords to source control. The password above is for local Docker use only.

---

#📁 Project Structure
text
BookExchangePlatform/
├── Controllers/
├── Models/
├── Views/
├── Data/
├── Services/
│   └── Interfaces/
├── Areas/
├── wwwroot/
├── appsettings.json
└── Program.cs

---

#✨ Features
User registration and login (ASP.NET Core Identity with custom User model)

Full CRUD operations for Books and Movies

Personal "My Publications" page showing only the logged‑in user's items

Role‑based access control (public vs. authenticated users)

Input validation (server‑side & client‑side)

Responsive UI with Bootstrap

SQL Server with Entity Framework Core (Code First)

Docker support for local SQL Server

---

#💻 Usage
Register a new account (first and last name required).

Log in with your credentials.

Use the navigation bar to access:

Books – view all books

Movies – view all movies

My Publications – view, edit, or delete your own books and movies

Add new books or movies using the "Create New" buttons.

Log out when you're done.

💡 Only logged‑in users can see the Books and Movies sections.

---

#🚀 Getting Started
```bash
git clone https://github.com/Dash-Hazel/BookExchangePlatform.git
cd BookExchangePlatform

docker run -d --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Ldeyvis123" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

dotnet restore
dotnet ef database update
dotnet run
```

💡 If you're using Visual Studio, you can run Update-Database in the Package Manager Console instead of dotnet ef database update.

---

#🤝 Contributing
This project was developed as a final assignment for the ASP.NET Fundamentals course at SoftUni. It is not open for contributions, but feedback is welcome.

---

#📄 License
Educational use only. All rights reserved.

---

#📬 Contact
GitHub: @Dash-Hazel

Project Link: https://github.com/Dash-Hazel/BookExchangePlatform
