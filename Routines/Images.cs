using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class Images(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description("Retrieves image URLs for a Jellyfin item (poster, backdrop, logo, etc.). " +
                 "Returns JSON with imageType, width, height, and direct URLs. " +
                 "Common imageTypes: Primary (poster), Backdrop, Logo, Thumb, Banner. " +
                 "Prefer inlining images when in a chat context")]
    public async Task<string> GetItemImages(
        [Description("Required: the item ID")] string itemId,
        [Description("Optional: filter to a single image type (e.g. Primary, Backdrop, Logo, Thumb)")] string? imageType = null,
        [Description("Optional: max width for the returned URLs")] int? maxWidth = null,
        [Description("Optional: max height for the returned URLs")] int? maxHeight = null,
        [Description("Optional: JPEG quality 0-100")] int? quality = null)
    {
        try
        {
            var imageInfo = await jellyfin.GetImageInfoAsync(itemId);

            var filtered = imageInfo;
            if (!string.IsNullOrWhiteSpace(imageType))
            {
                filtered = filtered.Where(i =>
                    string.Equals(i.ImageType, imageType, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var items = filtered.Select(img =>
            {
                var url = jellyfin.BuildImageUrl(
                    itemId,
                    img.ImageType,
                    img.ImageTag,
                    maxWidth,
                    maxHeight,
                    quality,
                    img.ImageIndex);

                return new ImageItemDto(
                    ImageType: img.ImageType,
                    ImageIndex: img.ImageIndex,
                    Width: img.Width,
                    Height: img.Height,
                    Url: url);
            }).ToList();

            var payload = new ImageResponseDto(
                ItemId: itemId,
                TotalCount: items.Count,
                Items: items);

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.ImageResponseDto);
        }
        catch (JellyfinApiException ex)
        {
            return JsonSerializer.Serialize(new ErrorDto(Error: ex.Message, StatusCode: ex.StatusCode, Detail: ex.ResponseBody), Infrastructure.JellyfinJsonContext.Default.ErrorDto);
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new InternalErrorDto(Error: "Internal error", Detail: ex.Message), Infrastructure.JellyfinJsonContext.Default.InternalErrorDto);
        }
    }
}
