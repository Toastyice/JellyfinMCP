using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class Actor(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description("Searches people in Jellyfin. If a name is provided, it searches by name; otherwise it returns all people. " +
                 "Search format is usually Lastname firstname. " +
                 "Returns JSON with query, totalCount, hasMore, nextStartIndex, and items containing basic person metadata.")]
    public async Task<string> GetDetailedPerson(
        [Description("Optional: the person name to search for. Usually format: 'Lastname firstname'")] string? name = null,
        [Description("Optional: max results")] int? limit = 10,
        [Description("Optional: zero-based start index for pagination")] int? startIndex = null)
    {
        try
        {
            var requestedLimit = limit ?? 10;
            var apiLimit = requestedLimit + 1;

            var response = await jellyfin.GetPersonsAsync(
                searchTerm: name,
                startIndex: startIndex,
                limit: apiLimit);

            var hasMore = response.Items.Count > requestedLimit;
            
            var items = response.Items
                .Take(requestedLimit)
                .Select(p => new PersonItemOutputDto(
                    Name: p.Name,
                    Id: p.Id,
                    Type: p.Type))
                .ToList();

            var payload = new PersonResponseDto(
                Query: name,
                TotalCount: response.TotalRecordCount,
                HasMore: hasMore,
                NextStartIndex: hasMore ? (startIndex ?? 0) + requestedLimit : null,
                Items: items);

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.PersonResponseDto);
        }
        catch (Exception ex)
        {
            return $"Error connecting to Jellyfin: {ex.Message}";
        }
    }
}
