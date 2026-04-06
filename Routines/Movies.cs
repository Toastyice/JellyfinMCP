using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class Movies(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description("Searches movies in Jellyfin by name. " +
                 "Optional filters: year, limit, startIndex, genre, personID, and parentId/library ID. " +
                 "Use the personID when searching for movies by a specific person." +
                 "Returns JSON with query, totalCount, hasMore, nextStartIndex, and items containing id, name, type, year, and runtimeMinutes.")]
    public async Task<string> SearchMovies(
        [Description("Optional: search text, " +
                     "Must not be used to search for a person an actor/actress")] string? searchTerm = null,
        [Description("Optional: year filter")] int? year = null,
        [Description("Optional: max results")] int? limit = 10,
        [Description("Optional: zero-based start index for pagination")] int? startIndex = null,
        [Description("Optional: Array of strings If specified, results will be filtered based on genre. This allows multiple, pipe delimited. " +
                     "always use this when all movies from with a specific genre should be returned!")] string? genre = null,
        [Description("Optional: person ID get from GetDetailedPerson, always use this when all movies from an actor should be returned!")] string? personid = null,
        [Description("Optional: library or parent item ID filter")] string? parentId = null)
    {
        try
        {
            var requestedLimit = limit ?? 10;
            var apiLimit = requestedLimit + 1;

            var response = await jellyfin.SearchItemsAsync(
                searchTerm: searchTerm,
                limit: apiLimit,
                startIndex: startIndex,
                includeItemTypes: "Movie",
                year: year,
                genre: genre,
                person: personid,
                parentId: parentId);

            var hasMore = response.Items.Count > requestedLimit;
            var items = response.Items
                .Take(requestedLimit)
                .Select(m => new MovieItemOutputDto(
                    Id: m.Id,
                    Name: m.Name,
                    Type: m.Type ?? "Movie",
                    Year: m.ProductionYear,
                    RuntimeMinutes: m.RunTimeTicks is null ? null : (int?)TimeSpan.FromTicks(m.RunTimeTicks.Value).TotalMinutes))
                .ToList();

            var payload = new MovieResponseDto(
                Query: searchTerm,
                TotalCount: response.TotalRecordCount ?? response.Items.Count,
                HasMore: hasMore,
                NextStartIndex: hasMore ? (int?)((startIndex ?? 0) + requestedLimit) : null,
                Items: items);

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.MovieResponseDto);
        }
        catch (Exception ex)
        {
            return $"Error connecting to Jellyfin: {ex.Message}";
        }
    }
}
