Interactive Game Curator

The Interactive Game Curator is a web application designed to provide personalized game recommendations. Users can input a list of games they enjoy, and the application leverages the Steam API and a Neo4j graph database to suggest related games based on what other users tend to play.

Features:

Backend:

    Built with ASP.NET Core Web API.
    Uses Neo4j as a graph database to model and query game relationships.
    Integrates with the Steam API to fetch game details.

Frontend (Planned):

    Built with React.
    Interactive and responsive user interface.
    Animated transitions using Framer Motion or React Spring.

Recommendations Engine:

    Analyzes relationships between games stored in Neo4j (e.g., "users who played this also played that").
    Provides curated lists of related games.

Technologies Used

Backend:
    
        ASP.NET Core Web API
        Neo4j (graph database)
        Serilog (structured logging)
        NUnit and NSubstitute (unit testing)

Frontend (Planned):

        React
        Axios (for API calls)
        SCSS (for styling)

Tools and Deployment:

        GitHub Actions (for CI/CD pipeline)
        AWS S3 (for frontend hosting)
        Docker (optional for containerization)

Setup Instructions
Prerequisites

    Install .NET 6 SDK or later.
    Install Neo4j (local or cloud instance).
    Obtain a Steam API key from the Steam Developer Portal.

Backend Setup

    Update appsettings.json with your Neo4j and Steam API credentials:

    "Neo4j": {
        "Uri": "bolt://localhost:7687",
        "Username": "neo4j",
        "Password": "your_password"
    },
    "SteamAPI": {
        "ApiKey": "your_steam_api_key"
    }
    
 Run the application:
    
    dotnet run

Access the API documentation via Swagger:

    http://localhost:<port>/swagger

Testing

Run the unit tests:
    
    dotnet test

Current Progress

    Backend:
        Game recommendation logic implemented with Neo4j.
        Steam API integrated for fetching game details.
        Unit tests written for GameService and repository methods.

    Frontend:
        React application setup planned.
        Component designs (SearchBar, GameList) in progress.

Future Enhancements

    Frontend:
        Create a responsive and animated user interface.
        Display recommended games with rich visuals.
        Implements structured logging with Serilog for monitoring and debugging.

    Backend:
        Structured logging added with Serilog.
        Add caching to improve performance for frequently queried data.
        Expand recommendation algorithms to include more criteria (e.g., genre, ratings).

    Deployment:
        Automate multi-environment deployments (Dev → Test → Prod) using GitHub Actions.
        Deploy the backend to AWS or Azure.
