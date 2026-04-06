namespace JellyfinMCP.Configuration;

public sealed class JellyfinOptions
{
    public required string BaseUrl { get; init; }
    public string? ApiKey { get; init; }
    public string? UserId { get; init; }

    public static JellyfinOptions FromEnvironment()
    {
        var baseUrl = Environment.GetEnvironmentVariable("JELLYFIN_URL") ?? "http://localhost:8096";
        var apiKey = Environment.GetEnvironmentVariable("JELLYFIN_API_KEY");
        var userId = Environment.GetEnvironmentVariable("JELLYFIN_USER_ID");

        return new JellyfinOptions
        {
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            UserId = userId
        };
    }
}