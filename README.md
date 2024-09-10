EventTicketAPI
EventTicketAPI is a web API for managing event tickets, including browsing events, purchasing tickets, and favoriting events. This project uses ASP.NET Core with Entity Framework Core, JWT Bearer Authentication, and Redis caching.

Features
Event Management: Add, update, delete, and browse events.
Ticket Management: Purchase and manage event tickets.
User Authentication: JWT-based registration and login.
Favoriting Events: Mark events as favorites for quick access.
Caching: Uses Redis for caching frequently accessed data.
Technologies Used
ASP.NET Core
Entity Framework Core
JWT Bearer Authentication
Redis: For caching.
SQL Server
Getting Started
Prerequisites
.NET 8 SDK
SQL Server
Redis
Installation
Clone the repository:

bash
Copy code
git clone https://github.com/GiorgiChekurishvili/EventTicketAPI.git
Navigate to the project directory:

bash
Copy code
cd EventTicketAPI
Restore dependencies:

bash
Copy code
dotnet restore
Update your appsettings.json with the correct connection strings:

json
Copy code
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EventTickets;TrustServerCertificate=True;Trusted_Connection=True;"
  },
  "RedisCache": {
    "ConnectionString": "localhost:1210"
  },
Apply database migrations:

bash
Copy code
dotnet ef database update
Run the application:

bash
Copy code
dotnet run
API Endpoints
Authentication
POST /api/auth/register: Register a new user.
POST /api/auth/login: Log in with JWT authentication.
Events
GET /api/events: Retrieve a list of events.
GET /api/events/{id}: Retrieve event details by ID.
POST /api/events: Create a new event.
PUT /api/events/{id}: Update an event by ID.
DELETE /api/events/{id}: Delete an event by ID.
Tickets
POST /api/tickets/purchase: Purchase a ticket for an event.
GET /api/tickets/user/{userId}: Get all tickets for a specific user.
DELETE /api/tickets/{ticketId}: Cancel a ticket purchase.
Favorites
POST /api/favorites/{eventId}: Add an event to favorites.
DELETE /api/favorites/{eventId}: Remove an event from favorites.
GET /api/favorites/user/{userId}: Get all favorited events for a user.
Caching
The application utilizes Redis for caching event data to improve performance.

Contributing
Feel free to contribute by opening pull requests or issues.

License
This project is licensed under the MIT License.
