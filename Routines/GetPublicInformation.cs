using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class PublicInformation(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description("Retrieves public Jellyfin server information. " +
                 "useful for checking if the server is online and available." +
                 "Returns JSON with basic server metadata such as serverName, version, productName, id, localAddress, and startupWizardCompleted.")]
    public async Task<string> GetPublicInformation()
    {
        try
        {
            var response = await jellyfin.GetPublicInformationAsync();

            var payload = new PublicInfoOutputDto(
                ServerName: response.ServerName,
                Version: response.Version,
                ProductName: response.ProductName,
                Id: response.Id,
                LocalAddress: response.LocalAddress,
                StartupWizardCompleted: response.StartupWizardCompleted);

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.PublicInfoOutputDto);
        }
        catch (Exception ex)
        {
            return $"Error connecting to Jellyfin: {ex.Message}";
        }
    }
}
