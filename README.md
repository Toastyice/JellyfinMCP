# JellyfinMCP

An MCP server for [Jellyfin](https://jellyfin.org/) media server integration. Exposes tools for searching movies, browsing libraries, retrieving genres, finding actors, getting movie details, and fetching item images — all over the Model Context Protocol (MCP).

## Features

- **Search movies** — by title, year, genre, actor, or library
- **Browse libraries** — list all Jellyfin library sections
- **Retrieve genres** — search and paginate through genres
- **Find actors** — search people by name with pagination
- **Movie details** — overview, cast, studios, media streams, ratings, and more
- **Image URLs** — get direct URLs for posters, backdrops, logos, and more
- **Server info** — check if your Jellyfin server is online and get metadata

## Prerequisites

- A running Jellyfin server
- A Jellyfin API key (optional, for unauthenticated servers)
- A Jellyfin user ID (required for user-scoped endpoints)

## Configuration

The server is configured via environment variables:

| Variable | Required | Default | Description |
|---|---|---|---|
| `JELLYFIN_URL` | No | `http://localhost:8096` | Base URL of your Jellyfin server |
| `JELLYFIN_API_KEY` | No | — | Jellyfin API key |
| `JELLYFIN_USER_ID` | Yes for user endpoints | — | Jellyfin user ID |

## Developing Locally

To test this MCP server from source code, configure your IDE to run the project directly using `dotnet run`:

```json
{
  "servers": {
    "JellyfinMCP": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "<PATH TO PROJECT DIRECTORY>"
      ],
      "env": {
        "JELLYFIN_URL": "http://localhost:8096",
        "JELLYFIN_API_KEY": "your-api-key",
        "JELLYFIN_USER_ID": "your-user-id"
      }
    }
  }
}
```

Refer to your IDE's MCP server documentation:

- [Use MCP servers in VS Code](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [Use MCP servers in Visual Studio](https://learn.microsoft.com/visualstudio/ide/mcp-servers)

## Building

### Native AOT Publish

The project is configured for Native AOT compilation. Publish for your target platform:

```bash
dotnet publish -c Release -r linux-x64
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r osx-arm64
```

### Docker

```bash
docker build -t jellyfin-mcp .
docker run --rm -e JELLYFIN_URL=http://your-server:8096 -e JELLYFIN_API_KEY=your-key -e JELLYFIN_USER_ID=your-user-id jellyfin-mcp
```

## Publishing to NuGet.org

1. Run `dotnet pack -c Release` to create the NuGet package
2. Publish with `dotnet nuget push bin/Release/*.nupkg --api-key <your-api-key> --source https://api.nuget.org/v3/index.json`

## Using from NuGet.org

Create an `mcp.json` file in your IDE workspace:

```json
{
  "servers": {
    "JellyfinMCP": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "JellyfinMCP",
        "--version",
        "0.1.0-beta",
        "--yes"
      ],
      "env": {
        "JELLYFIN_URL": "http://localhost:8096",
        "JELLYFIN_API_KEY": "your-api-key",
        "JELLYFIN_USER_ID": "your-user-id"
      }
    }
  }
}
```

## Available Tools

| Tool | Description |
|---|---|
| `SearchMovies` | Search movies by name, year, genre, actor, or library |
| `GetDetailedMovie` | Get full movie details by ID |
| `GetLibraries` | List all Jellyfin library sections |
| `GetGenres` | Browse and search genres |
| `GetDetailedPerson` | Search actors/people by name |
| `GetItemImages` | Get image URLs for any Jellyfin item |
| `GetPublicInformation` | Check server status and metadata |

## More Information

.NET MCP servers use the [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol) C# SDK.

- [Official MCP Documentation](https://modelcontextprotocol.io/)
- [Protocol Specification](https://spec.modelcontextprotocol.io/)
- [GitHub Organization](https://github.com/modelcontextprotocol)
- [MCP C# SDK](https://csharp.sdk.modelcontextprotocol.io/)
