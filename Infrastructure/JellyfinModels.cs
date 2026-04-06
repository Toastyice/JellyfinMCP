using System.Text.Json.Serialization;

namespace JellyfinMCP.Infrastructure;

public sealed record JellyfinSearchResponse(
    [property: JsonPropertyName("Items")] List<JellyfinItem> Items,
    [property: JsonPropertyName("TotalRecordCount")] int? TotalRecordCount = null);

public sealed record JellyfinItem(
    [property: JsonPropertyName("Name")] string? Name,
    [property: JsonPropertyName("Id")] string? Id,
    [property: JsonPropertyName("Type")] string? Type = null,
    [property: JsonPropertyName("ProductionYear")] int? ProductionYear = null,
    [property: JsonPropertyName("Overview")] string? Overview = null,
    [property: JsonPropertyName("RunTimeTicks")] long? RunTimeTicks = null,
    [property: JsonPropertyName("CommunityRating")] double? CommunityRating = null,
    [property: JsonPropertyName("IsFavorite")] bool? IsFavorite = null);

public sealed record JellyfinMovieDetails(
    [property: JsonPropertyName("Id")] string? Id,
    [property: JsonPropertyName("Name")] string? Name,
    [property: JsonPropertyName("Type")] string? Type,
    [property: JsonPropertyName("ProductionYear")] int? ProductionYear,
    [property: JsonPropertyName("Overview")] string? Overview,
    [property: JsonPropertyName("Genres")] List<string>? Genres,
    [property: JsonPropertyName("Studios")] List<JellyfinStudio>? Studios,
    [property: JsonPropertyName("People")] List<JellyfinPerson>? People,
    [property: JsonPropertyName("ProviderIds")] Dictionary<string, string>? ProviderIds,
    [property: JsonPropertyName("Path")] string? Path,
    [property: JsonPropertyName("MediaStreams")] List<JellyfinMediaStream>? MediaStreams,
    [property: JsonPropertyName("CommunityRating")] double? CommunityRating);

public sealed record JellyfinStudio(
    [property: JsonPropertyName("Name")] string? Name);

public sealed record JellyfinPerson(
    [property: JsonPropertyName("Name")] string? Name,
    [property: JsonPropertyName("Role")] string? Role,
    [property: JsonPropertyName("Type")] string? Type);

public sealed record JellyfinMediaStream(
    [property: JsonPropertyName("Codec")] string? Codec,
    [property: JsonPropertyName("Type")] string? Type,
    [property: JsonPropertyName("Language")] string? Language,
    [property: JsonPropertyName("DisplayTitle")] string? DisplayTitle);

public sealed record JellyfinLibraryResponse(
    [property: JsonPropertyName("Items")] List<JellyfinLibrary> Items);

public sealed record JellyfinLibrary(
    [property: JsonPropertyName("Name")] string? Name,
    [property: JsonPropertyName("Id")] string? Id,
    [property: JsonPropertyName("CollectionType")] string? CollectionType);

public sealed record JellyfinPublicInfo(
    [property: JsonPropertyName("ServerName")] string? ServerName,
    [property: JsonPropertyName("Version")] string? Version,
    [property: JsonPropertyName("ProductName")] string? ProductName,
    [property: JsonPropertyName("Id")] string? Id,
    [property: JsonPropertyName("LocalAddress")] string? LocalAddress,
    [property: JsonPropertyName("StartupWizardCompleted")] bool? StartupWizardCompleted);
    
public sealed record JellyfinGenreResponse(
    [property: JsonPropertyName("Items")] List<JellyfinGenreItem> Items,
    [property: JsonPropertyName("TotalRecordCount")] int TotalRecordCount);

public sealed record JellyfinGenreItem(
    [property: JsonPropertyName("Name")] string Name,
    [property: JsonPropertyName("Id")] string Id);

public sealed record JellyfinImageInfoResponse(
    [property: JsonPropertyName("Images")] List<JellyfinImageInfo> Images);

public sealed record JellyfinImageInfo(
    [property: JsonPropertyName("ImageType")] string ImageType,
    [property: JsonPropertyName("ImageIndex")] int? ImageIndex = null,
    [property: JsonPropertyName("ImageTag")] string? ImageTag = null,
    [property: JsonPropertyName("Width")] int? Width = null,
    [property: JsonPropertyName("Height")] int? Height = null);

public sealed record JellyfinPersonsResponse(
    [property: JsonPropertyName("Items")] List<JellyfinPersonItem> Items,
    [property: JsonPropertyName("TotalRecordCount")] int TotalRecordCount);

public sealed record JellyfinPersonItem(
    [property: JsonPropertyName("Name")] string Name,
    [property: JsonPropertyName("Id")] string Id,
    [property: JsonPropertyName("Type")] string? Type = null);