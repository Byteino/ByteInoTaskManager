# ByteIno Task Manager

A simple yet functional **Task Management System** built with **ASP.NET Core MVC** and **Entity Framework Core**.  
This project includes both a **user panel** and an **admin panel** with role-based authentication.

---

## Features

### User Panel
- User registration & login with **Identity**
- Create, edit, and delete **Tasks**
- Mark tasks as **Pending** or **Finished**
- Create and manage **Categories**
- Assign tasks to categories
- View personal dashboard with task list and categories

### Admin Panel
- List all users with:
  - Email
  - Active/Inactive status
  - Number of Tasks
  - Number of Categories
- Activate/Deactivate users
- Edit user details
- Delete users (with confirmation modal)
- View user-related tasks and categories in detail

---

## Technologies Used
- **ASP.NET Core 9.0 (MVC)**
- **Entity Framework Core**
- **Identity Framework** (Authentication & Roles)
- **AutoMapper**
- **Bootstrap 5** (UI/Styling)
- **SQL Server** (Database)

---

## Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Byteino/ByteInoTaskManager.git
   cd ByteInoTaskManager


2. Open the project in Visual Studio 2022 (or later)

3. Restore NuGet packages
	Tools -> NuGet Package Manager -> Restore Packages

4. Update database
	Open Package Manager Console
	Run: Update-Database
	This will create the database with required tables and seed the default admin user.

5. Run the project
	Press F5 or click Run in Visual Studio