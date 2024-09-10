# EventTicketAPI

EventTicketAPI is a web API for managing event tickets, including browsing events, purchasing tickets, and favoriting events. This project uses ASP.NET Core with Entity Framework Core, JWT Bearer Authentication, and Redis caching.

## Features

- **Event Management**: Add, update, delete, and browse events.
- **Ticket Management**: Purchase and manage event tickets.
- **User Authentication**: JWT-based registration and login, verification using smtp client to send emails to verity user authentication.
- **Favoriting Events**: Mark events as favorites for quick access.
- **Caching**: Uses Redis for caching frequently accessed data.

## Technologies Used

- **ASP.NET Core**
- **Entity Framework Core**
- **JWT Bearer Authentication**
- **Redis**: For caching.
- **SQL Server**
- **AutoMapper**
- **Swagger**

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/)

### Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/GiorgiChekurishvili/EventTicketAPI.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd EventTicketAPI
    ```

3. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

4. **Update your `appsettings.json` with the correct connection strings:**

    ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EventTickets;TrustServerCertificate=True;Trusted_Connection=True;"
    },
    "RedisCache": {
    "ConnectionString": "localhost:1210"
    }
    ```

    - **SQL Server**: Ensure you have a SQL Server instance running locally or remotely.
    - **Redis**: The Redis connection string is typically set to `localhost:6379` if running locally.

5. **Apply database migrations:**

    ```bash
    dotnet ef database update
    ```

6. **Run the application:**

    ```bash
    dotnet run
    ```

## API Endpoints

### Authentication

- **POST** `/api/auth/register`: Register a new user.
- **POST** `/api/auth/login`: Log in with JWT authentication.

### Events

- **GET** `/api/events`: Retrieve a list of events.
- **GET** `/api/events/{id}`: Retrieve event details by ID.
- **POST** `/api/events`: Create a new event.
- **PUT** `/api/events/{id}`: Update an event by ID.
- **DELETE** `/api/events/{id}`: Delete an event by ID.

### Tickets

- **POST** `/api/tickets/purchase`: Purchase a ticket for an event.
- **GET** `/api/tickets/user/{userId}`: Get all tickets for a specific user.
- **DELETE** `/api/tickets/{ticketId}`: Cancel a ticket purchase.

### Favorites

- **POST** `/api/favorites/{eventId}`: Add an event to favorites.
- **DELETE** `/api/favorites/{eventId}`: Remove an event from favorites.
- **GET** `/api/favorites/user/{userId}`: Get all favorited events for a user.

## Caching

The application utilizes Redis for caching event data to improve performance.

## Contributing

Feel free to contribute by opening pull requests or issues. Your contributions are welcome!

## Security
JWT authentication is used for securing the endpoints.
Different user roles like Admin and Member are managed.
## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

