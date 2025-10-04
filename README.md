eCommerceSolutionUserService
🛍️ Overview

The eCommerceSolutionUserService is a backend service designed to manage user-related functionalities in an e-commerce platform. Built using C#, this service handles user authentication, profile management, and integrates seamlessly with other components of the e-commerce solution.

🚀 Features

User Registration & Authentication: Secure user sign-up and login mechanisms.

Profile Management: Allows users to update personal information and preferences.

Integration Ready: Easily connects with other services like product catalogs and order management.

🧰 Technologies Used

C#: Primary programming language.

ASP.NET Core: Framework for building the API.

Entity Framework Core: ORM for database interactions.

Docker: Containerization for consistent deployment.

📦 Getting Started
Prerequisites

Ensure you have the following installed:

.NET SDK
 (version 6.0 or higher)

Docker

Installation

Clone the repository:

git clone https://github.com/BenBormey/eCommerceSolutionUserService.git
cd eCommerceSolutionUserService


Restore dependencies:

dotnet restore


Build the solution:

dotnet build

Running the Service

To run the service locally:

dotnet run


Alternatively, to run using Docker:

docker build -t ecommerce-userservice .
docker run -p 5000:80 ecommerce-userservice


The service will be available at http://localhost:5000.

🧪 Testing

Unit tests are located in the eCommerceSolutionUserService.Tests directory. To run them:

dotnet test

🤝 Contributing

Contributions are welcome! Please fork the repository, create a new branch, and submit a pull request with your proposed changes.

📄 License

This project is licensed under the MIT License - see the LICENSE
 file for details.
