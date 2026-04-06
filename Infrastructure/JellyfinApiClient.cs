using System.Text.Json;
using JellyfinMCP.Configuration;
using JellyfinMCP.Infrastructure;

namespace JellyfinMCP.Infrastructure;

public sealed class JellyfinApiClient(HttpClient http, JellyfinOptions options)
{
    private readonly string? _userId = options.UserId;

    private static string BuildQueryString(IDictionary<string, string?> parameters)
    {
        var parts = parameters
            .Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!)}");

        return parts.Any() ? "?" + string.Join("&", parts) : string.Empty;
    }

    private async Task<JellyfinSearchResponse> GetSearchResponseAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.JellyfinSearchResponse);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<JellyfinGenreResponse> GetGenreResponseAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.JellyfinGenreResponse);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<JellyfinLibraryResponse> GetLibraryResponseAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.JellyfinLibraryResponse);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<JellyfinPublicInfo> GetPublicInfoAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.JellyfinPublicInfo);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<JellyfinMovieDetails> DeserializeMovieDetailsAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.JellyfinMovieDetails);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<List<JellyfinImageInfo>> GetImageInfoListAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.ListJellyfinImageInfo);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<JellyfinPersonsResponse> GetPersonsResponseAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
            throw new JellyfinApiException("Jellyfin returned an empty response.");

        var result = JsonSerializer.Deserialize(payload, JellyfinJsonContext.Default.JellyfinPersonsResponse);
        return result ?? throw new JellyfinApiException("Failed to deserialize Jellyfin response.");
    }

    private async Task<string> GetStringAsync(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var response = await http.GetAsync(relativeUrl, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<JellyfinSearchResponse> SearchItemsAsync(
        string? query = null,
        string? searchTerm = null,
        int? limit = null,
        int? startIndex = null,
        string? includeItemTypes = null,
        int? year = null,
        string? genre = null,
        string? person = null,
        string? parentId = null,
        CancellationToken cancellationToken = default)
    {
        var path = "/Items" + BuildQueryString(new Dictionary<string, string?>
        {
            ["Recursive"] = "true",
            ["SearchTerm"] = searchTerm ?? query,
            ["Limit"] = limit?.ToString(),
            ["StartIndex"] = startIndex?.ToString(),
            ["IncludeItemTypes"] = includeItemTypes,
            ["Years"] = year?.ToString(),
            ["Genres"] = genre,
            ["PersonIds"] = person,
            ["ParentId"] = parentId
        });

        return await GetSearchResponseAsync(path, cancellationToken);
    }
    
    public async Task<JellyfinGenreResponse> GetGenresAsync(
        string? searchTerm = null,
        int? startIndex = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var path = "/Genres" + BuildQueryString(new Dictionary<string, string?>
        {
            ["SearchTerm"] = searchTerm,
            ["StartIndex"] = startIndex?.ToString(),
            ["Limit"] = limit?.ToString(),
            ["Recursive"] = "true",
            ["UserId"] = GetUserIdOrThrow()
        });

        return await GetGenreResponseAsync(path, cancellationToken);
    }
    
    public async Task<JellyfinLibraryResponse> GetLibrariesAsync(CancellationToken cancellationToken = default)
        => await GetLibraryResponseAsync("/Users/" + GetUserIdOrThrow() + "/Views", cancellationToken);

    public async Task<JellyfinPublicInfo> GetPublicInformationAsync(CancellationToken cancellationToken = default)
        => await GetPublicInfoAsync("/system/info/public", cancellationToken);

    public async Task<JellyfinMovieDetails> GetMovieDetailsAsync(string movieId, CancellationToken cancellationToken = default)
    {
        var path = $"/Users/{GetUserIdOrThrow()}/Items/{Uri.EscapeDataString(movieId)}" + BuildQueryString(new Dictionary<string, string?>
        {
            ["Fields"] = "Overview,Genres,ProductionYear,Studios,People,ProviderIds,Path,MediaStreams,CommunityRating"
        });

        return await DeserializeMovieDetailsAsync(path, cancellationToken);
    }

    public async Task<string> GetItemRawAsync(string itemId, string fields, CancellationToken cancellationToken = default)
    {
        var path = $"/Users/{GetUserIdOrThrow()}/Items/{Uri.EscapeDataString(itemId)}{BuildQueryString(new Dictionary<string, string?>
        {
            ["Fields"] = fields
        })}";

        return await GetStringAsync(path, cancellationToken);
    }

    public async Task<List<JellyfinImageInfo>> GetImageInfoAsync(string itemId, CancellationToken cancellationToken = default)
        => await GetImageInfoListAsync($"/Items/{Uri.EscapeDataString(itemId)}/Images", cancellationToken);

    public async Task<JellyfinPersonsResponse> GetPersonsAsync(
        string? searchTerm = null,
        int? startIndex = null,
        int? limit = null,
        CancellationToken cancellationToken = default)
    {
        var path = "/Persons" + BuildQueryString(new Dictionary<string, string?>
        {
            ["SearchTerm"] = searchTerm,
            ["StartIndex"] = startIndex?.ToString(),
            ["Limit"] = limit?.ToString()
        });

        return await GetPersonsResponseAsync(path, cancellationToken);
    }

    public string BuildImageUrl(string itemId, string imageType, string? tag = null, int? maxWidth = null, int? maxHeight = null, int? quality = null, int? imageIndex = null)
    {
        var path = imageIndex is > 0
            ? $"/Items/{Uri.EscapeDataString(itemId)}/Images/{Uri.EscapeDataString(imageType)}/{imageIndex}"
            : $"/Items/{Uri.EscapeDataString(itemId)}/Images/{Uri.EscapeDataString(imageType)}";
        var query = BuildQueryString(new Dictionary<string, string?>
        {
            ["maxWidth"] = maxWidth?.ToString(),
            ["maxHeight"] = maxHeight?.ToString(),
            ["quality"] = quality?.ToString(),
            ["tag"] = tag
        });
        return http.BaseAddress?.ToString().TrimEnd('/') + path + query;
    }

    private string GetUserIdOrThrow() =>
        !string.IsNullOrWhiteSpace(_userId) ? _userId : throw new JellyfinApiException("JELLYFIN_USER_ID is required for user-scoped Jellyfin endpoints.");

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new JellyfinApiException(
            $"Jellyfin request failed with HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).",
            response.StatusCode,
            body);
    }
}
