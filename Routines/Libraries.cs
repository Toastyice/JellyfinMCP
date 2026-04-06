using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class Libraries(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description("Retrieves a list of Jellyfin library sections. " +
                 "Returns JSON with totalCount and items containing name, id, and collectionType.")]
    public async Task<string> GetLibraries()
    {
        try
        {
            var response = await jellyfin.GetLibrariesAsync();

            var payload = new LibraryResponseDto(
                TotalCount: response.Items.Count,
                Items: response.Items.Select(l => new LibraryItemOutputDto(
                    Name: l.Name,
                    Id: l.Id,
                    CollectionType: l.CollectionType)).ToList());

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.LibraryResponseDto);
        }
        catch (Exception ex)
        {
            return $"Error connecting to Jellyfin: {ex.Message}";
        }
    }
}
