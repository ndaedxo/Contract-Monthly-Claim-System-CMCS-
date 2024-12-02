[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/17MQ9l9o)


---

# Contract Monthly Claim System (CMCS)

## Overview

The **Contract Monthly Claim System (CMCS)** is a web-based platform designed to streamline the claim management process for lecturers and HR personnel. By automating and centralizing claim submissions, approvals, and document management, CMCS improves efficiency, transparency, and user experience for both lecturers and HR departments.

---



## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [System Architecture](#system-architecture)
- [Installation](#installation)
- [Usage](#usage)
- [Future Enhancements](#future-enhancements)
- [Contact](#contact)

---

## Features

- **Lecturer Dashboard**:
  - Submit claims for work done in a given month.
  - Upload supporting documents for claims (e.g., receipts, work verification).
  - Track the status of submitted claims (pending, approved, rejected).
  - Provide feedback on the claims process.

- **HR Dashboard**:
  - Review and approve or reject claims submitted by lecturers.
  - Generate invoices for approved claims.
  - Update lecturer data (e.g., personal information, claim details).

- **Real-time Notifications**:
  - Notifications for status updates (approved, rejected, etc.) are sent to both lecturers and HR in real-time using SignalR.

- **Admin Dashboard** (Optional):
  - Manage claims and users.
  - Generate reports and analytics based on claim data.

---

## Technologies Used

- **Frontend**: 
  - **Razor Pages**: Server-side rendering with dynamic page updates.
  - **Bootstrap**: Responsive design framework for mobile and desktop compatibility.

- **Backend**:
  - **ASP.NET Core MVC**: Handles application logic, controllers, and views.
  - **Entity Framework**: Object-Relational Mapping (ORM) framework for database interactions.

- **Database**:
  - **SQL Server**: Stores claim data, lecturer profiles, claim status, and documents.

- **Real-Time Updates**:
  - **SignalR**: Provides real-time notifications to users regarding claim statuses and document uploads.

---

## System Architecture

The system is composed of three main user roles:

1. **Lecturer**:
   - Submits claims, uploads documents, and tracks claim status.
   
2. **HR Personnel**:
   - Reviews submitted claims, approves or rejects them, and generates invoices.

3. **Admin/Academic Manager**:
   - Verifies claims before final approval, generates reports on claims, and handles user management.

Data flow in the system includes:
- Claims submission by lecturers, review and approval by HR, and status updates provided to all users in real-time.

---

## Installation

### Prerequisites

- **.NET 6.0** or later
- **SQL Server** (can use local DB for development)
- **Visual Studio** or **Visual Studio Code** with C# extension

### Steps to Install

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/ndaedxo/Contract-Monthly-Claim-System-CMCS-.git
   cd CMCS
   ```

2. Install the necessary NuGet packages:

   ```bash
   dotnet restore
   ```

3. Set up the database:

   - Update your `appsettings.json` file to include your SQL Server connection string.
   - Run migrations to set up the database schema:

     ```bash
     dotnet ef database update
     ```

4. Build and run the project:

   ```bash
   dotnet build
   dotnet run
   ```

5. Access the application in your browser:

   ```bash
   http://localhost:5000
   ```

---

## Usage

Once the system is up and running, users can:

- **Lecturers**:
  - Log in, submit claims, upload documents, and track claim statuses.
  
- **HR Personnel**:
  - Review and approve claims, generate invoices, and manage lecturer profiles.
  
- **Admin/Managers**:
  - Verify claims, generate reports, and manage system configurations.

---

## Future Enhancements

- **Mobile App**:
  - Extending functionality to mobile platforms (iOS/Android) to allow users to submit and track claims on the go.
  
- **Advanced Reporting**:
  - Implement detailed analytics and reporting tools for HR managers to better track claim trends, approval times, and lecturer activities.
  
- **Multilingual Support**:
  - Support for multiple languages to make the system more accessible to a broader user base.

---

## 👤 Author

Ndaedzo Austin Mukhuba
- GitHub: [@ndaedzo](https://github.com/ndaedxo)
- LinkedIn: [Ndaedzo Austin Mukhuba](https://linkedin.com/in/ndaedzo-mukhuba-71759033b)
- Email: [ndaemukhuba](ndaemukhuba@gmail.com)

---

This README provides a clear and concise overview of your system, the installation steps, and how users can interact with the CMCS platform.
