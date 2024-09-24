
# School Management App

## Overview

The School Management App is a comprehensive portal designed to streamline the management of school operations. Built with a robust technology stack, this application provides dedicated functionalities for both administrators and students, ensuring an efficient educational experience.

## Technology Stack

- **Frontend**: [Next.js 14](https://nextjs.org/)
- **Backend**: [ASP.NET 8](https://dotnet.microsoft.com/apps/aspnet)
- **Database**: [PostgreSQL](https://www.postgresql.org/)
- **Chatbot Integration**: [Flowise](https://flowise.ai/) using Gemini
- **Authentication**: JWT (JSON Web Tokens)
- **Security**: [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) for database credentials

## Features

### Authentication and Authorization

- Secure authentication for users with role-based access:
  - **Admin**: Full access to manage school operations.
  - **Student**: Access to personal profile and course management.

### Admin Functionalities

Admins have exclusive access to the following features:
- **Manage Courses**: Add, update, and delete courses.
- **Manage Semesters**: Create and manage academic semesters.
- **View Student Data**: Access and view detailed information on registered students.
- **Send Reminders**: Communicate important updates and reminders to students.

### Student Functionalities

Students can perform the following actions:
- **View Profile**: Access their personal profile and academic information.
- **Manage Courses**: Add or remove courses as per their academic requirements.

## Security

To ensure the security of sensitive information:
- Database credentials are stored securely in Azure Key Vault.
- User authentication is managed via JWT tokens to maintain session integrity and security.

## Installation

### Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (version 14 or above)
- [PostgreSQL](https://www.postgresql.org/download/) installed and running
- [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) for credential management

### Clone the Repository

```bash
git clone https://github.com/zainababid94/Cchool-Management-App
cd School-Management-App

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend
2. Restore the dependencies
   dotnet restore
3. Set up the database and run migrations:
   dotnet ef database update
4. Start the backend server:
   dotnet run

### Frontend Setup

1. Navigate to the frontend directory:
   cd frontend
2. Install the dependencies:
   npm install
3. Start the frontend application:
   npm run dev

### License
This project is licensed under the MIT License - see the LICENSE file for details.
