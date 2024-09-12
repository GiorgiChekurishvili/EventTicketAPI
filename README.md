# EventTicketAPI

EventTicketAPI is a web API for managing event tickets, including browsing events, purchasing tickets, and favoriting events. This project uses ASP.NET Core with Entity Framework Core, JWT Bearer Authentication, Redis caching and SMTP Client for Email Sending.

## Features

- **Event Management**: Add, update, delete, and browse events.
- **Ticket Management**: Purchase and manage event tickets.
- **User Authentication**: JWT-based registration and login.
- **Favoriting Events**: Mark events as favorites for quick access.
- **Caching**: Uses Redis for caching frequently accessed data.
- **MailSending**: Uses SMTP client and Mailkit library for EmailSending bought ticket information and verification token to verify the latest registered user
- **ImageUpload/Retrieve**: uses byte to upload image for different events and retrieve them.

## Technologies Used

- **ASP.NET Core**
- **Entity Framework Core**
- **JWT Bearer Authentication**
- **Redis**: For caching.
- **SQL Server**
- **AutoMapper**
- **Swagger**
- **MailKit**

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

- **POST** `/api/Authentication/register`: Register a new user.
- **POST** `/api/Authentication/login`: Log in with JWT authentication.
- **POST** `/api/Authentication/verify/{token}`: Verify user based on token send to their registered email.
- **POST** `/api/Authentication/forgetpassword/{email}`: Send token to email to verify if user wants to change password.
- **POST** `/api/Authentication/changepassword`: Change password using the token was sent from forgetpassword request

### Events

- **GET** `/api/Event/events`: Retrieve a list of events.
- **GET** `/api/Event/eventsbyid/{id}`: Retrieve event details by ID.
- **POST** `/api/Event/publishevent`: Create a new event.
- **PUT** `/api/Event/updatevent/{id}`: Update an event by ID.
- **DELETE** `/api/Event/deleteevent/{id}`: Delete an event by ID.
- **GET** `/api/Event/eventcategories`: Retrieve a list of event categories.
- **GET** `/api/Event/eventsbycategory/{categoryid}`: Retrieve events details by categories.

### Tickets

- **POST** `/api/Ticket/buyticket/{Eventid}/{TicketTypeId}/{TicketQuantity}`: Purchase a ticket for an event.
- **GET** `/api/tickets/viewmytickets`: Get all tickets for a specific user.
- **DELETE** `/api/Ticket/refundticket/{ticketId}`: Cancel a ticket purchase.
- 
### TicketType

- **GET** `/api/TicketType/seetickettypes/{eventId}`: view what tickets are available on a event.
- **POST** `/api/TicketType/addtickettype`: Create a new ticket type.
- **PUT** `/api/TicketType/changetickettype/{tickettypeId}`: Update a ticket.
- **DELETE** `/api/TicketType/removetickettype{TicketTypeId}/{EventId}`: Delete a ticket type by ID and eventId.

### Favorites

- **POST** `/api/Favorite/favoriteevent/{eventId}`: Add an event to favorites.
- **DELETE** `/api/Favorite/unfavoriteevent/{eventId}`: Remove an event from favorites.
- **GET** `/api/Favorite/viewmyfavorites/`: Get all favorited events for a user.
- 
- ### Images

- **POST** `/api/Images/UploadImage/{eventId}`: Upload an image for an event.
- **GET** `/api/Images/RetrieveImage/{eventId}`: RetrieveImage for a specific event.
- **DELETE** `/api/Images/DeleteImage/{eventId}`: Delete an image.

- - ### Transaction

- **POST** `/api/Transaction/maketransaction/{amount}`: Fill a balance.
- **GET** `/api/Transaction/{viewmybalance}`: View a balance.
- **GET** `/api/Transaction/viewmytransactions`: View transactions made by users for example buying tickets or filling a balance.


## Caching

The application utilizes Redis for caching event data to improve performance.

## Contributing

Feel free to contribute by opening pull requests or issues. Your contributions are welcome!

## Security
JWT authentication is used for securing the endpoints.
Different user roles like Admin and Member are managed.

