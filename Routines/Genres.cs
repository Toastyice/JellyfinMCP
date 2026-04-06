using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class Genres(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description("Retrieves a list of Jellyfin genres. " +
                 "Optionally provide a genre name to look up its specific ID. " +
                 "Returns JSON with query, totalCount, hasMore, nextStartIndex, and items containing name and id.")]
    public async Task<string> GetGenres(
        [Description("Optional. The specific name of the genre to look up (e.g., 'Action', 'Sci-Fi').")] string? genreName = null,
        [Description("Optional: max results")] int? limit = 10,
        [Description("Optional: zero-based start index for pagination")] int? startIndex = null)
    {
        try
        {
            var requestedLimit = limit ?? 10;
            var apiLimit = requestedLimit + 1;

            var response = await jellyfin.GetGenresAsync(
                searchTerm: genreName,
                startIndex: startIndex,
                limit: apiLimit);

            var hasMore = response.Items.Count > requestedLimit;
            
            var items = response.Items
                .Take(requestedLimit)
                .Select(g => new GenreItemOutputDto(
                    Name: g.Name,
                    Id: g.Id))
                .ToList();

            var payload = new GenreResponseDto(
                Query: genreName,
                TotalCount: response.TotalRecordCount,
                HasMore: hasMore,
                NextStartIndex: hasMore ? (int?)((startIndex ?? 0) + requestedLimit) : null,
                Items: items);

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.GenreResponseDto);
        }
        catch (Exception ex)
        {
            return $"Error connecting to Jellyfin: {ex.Message}";
        }
    }
}
