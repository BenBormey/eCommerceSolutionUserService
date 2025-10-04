CleanCity Booking Service 🧹
Overview

CleanCity Booking Service is a full-stack web application designed for managing cleaning service bookings. Customers can select service plans, schedule dates, choose locations, and make secure payments. The system supports recurring bookings with automatic end-date calculation and partial or full payment options via QR code.

Features
Booking & Plans

One-time, Bi-weekly (8% discount), Monthly (15% discount) plans.

Automatic end-date calculation for recurring bookings.

Add customer notes and detailed address.

Customer Management

Secure login and authentication for customers.

Profile management with booking history.

Payment Integration

Partial (30%) upfront payment or full payment.

QR code for payment simulation.

Service & Location Management

List of cleaning services with price and quantity selection.

Predefined locations with the option to add new ones.

Technology Stack

Frontend: Vue 3, Vue Router, Composition API, Pinia

Backend: ASP.NET Core Web API

Database: SQL Server

Authentication: JWT-based login

Payment: QR code simulation

State Management: Pinia (for cart management)

Getting Started
Prerequisites

.NET 8 SDK

Node.js 18+

SQL Server (or compatible database)

Installation

Clone the repository:

git clone https://github.com/BenBormey/eCommerceSolutionUserService.git
cd eCommerceSolutionUserService


Install frontend dependencies:

cd client
npm install


Run the frontend:

npm run dev


Run the backend:

cd ../server
dotnet restore
dotnet run


Access the app at:

Frontend: http://localhost:5173

API: http://localhost:5000

Booking Flow

Login or register as a customer.

Select a cleaning plan (One-time, Bi-weekly, Monthly).

Choose location and add detailed address.

Select date & time for service.

Review summary and choose payment option (30% partial or 100% full).

Confirm booking and scan QR for payment.

Testing

Run backend tests:

cd server
dotnet test

Contribution

Contributions are welcome!

Fork the repository

Create your feature branch (git checkout -b feature/my-feature)

Commit your changes (git commit -am 'Add new feature')

Push to the branch (git push origin feature/my-feature)

Open a Pull Request
