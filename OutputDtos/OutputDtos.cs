using System.Text.Json.Serialization;

namespace JellyfinMCP.OutputDtos;

public sealed record ImageItemDto(
    [property: JsonPropertyName("imageType")] string ImageType,
    [property: JsonPropertyName("imageIndex")] int? ImageIndex,
    [property: JsonPropertyName("width")] int? Width,
    [property: JsonPropertyName("height")] int? Height,
    [property: JsonPropertyName("url")] string Url);

public sealed record ImageResponseDto(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("totalCount")] int TotalCount,
    [property: JsonPropertyName("items")] List<ImageItemDto> Items);

public sealed record ErrorDto(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("statusCode")] System.Net.HttpStatusCode? StatusCode,
    [property: JsonPropertyName("detail")] string? Detail);

public sealed record InternalErrorDto(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("detail")] string Detail);

public sealed record PersonItemOutputDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string? Type);

public sealed record PersonResponseDto(
    [property: JsonPropertyName("query")] string? Query,
    [property: JsonPropertyName("totalCount")] int TotalCount,
    [property: JsonPropertyName("hasMore")] bool HasMore,
    [property: JsonPropertyName("nextStartIndex")] int? NextStartIndex,
    [property: JsonPropertyName("items")] List<PersonItemOutputDto> Items);

public sealed record MovieItemOutputDto(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("year")] int? Year,
    [property: JsonPropertyName("runtimeMinutes")] int? RuntimeMinutes);

public sealed record MovieResponseDto(
    [property: JsonPropertyName("query")] string? Query,
    [property: JsonPropertyName("totalCount")] int TotalCount,
    [property: JsonPropertyName("hasMore")] bool HasMore,
    [property: JsonPropertyName("nextStartIndex")] int? NextStartIndex,
    [property: JsonPropertyName("items")] List<MovieItemOutputDto> Items);

public sealed record MovieDetailsOutputDto(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("year")] int? Year,
    [property: JsonPropertyName("overview")] string? Overview,
    [property: JsonPropertyName("genres")] List<string>? Genres,
    [property: JsonPropertyName("studios")] List<Infrastructure.JellyfinStudio>? Studios,
    [property: JsonPropertyName("people")] List<Infrastructure.JellyfinPerson>? People,
    [property: JsonPropertyName("providerIds")] Dictionary<string, string>? ProviderIds,
    [property: JsonPropertyName("path")] string? Path,
    [property: JsonPropertyName("mediaStreams")] List<Infrastructure.JellyfinMediaStream>? MediaStreams,
    [property: JsonPropertyName("communityRating")] double? CommunityRating);

public sealed record LibraryItemOutputDto(
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("collectionType")] string? CollectionType);

public sealed record LibraryResponseDto(
    [property: JsonPropertyName("totalCount")] int TotalCount,
    [property: JsonPropertyName("items")] List<LibraryItemOutputDto> Items);

public sealed record PublicInfoOutputDto(
    [property: JsonPropertyName("serverName")] string? ServerName,
    [property: JsonPropertyName("version")] string? Version,
    [property: JsonPropertyName("productName")] string? ProductName,
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("localAddress")] string? LocalAddress,
    [property: JsonPropertyName("startupWizardCompleted")] bool? StartupWizardCompleted);

public sealed record GenreItemOutputDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("id")] string Id);

public sealed record GenreResponseDto(
    [property: JsonPropertyName("query")] string? Query,
    [property: JsonPropertyName("totalCount")] int TotalCount,
    [property: JsonPropertyName("hasMore")] bool HasMore,
    [property: JsonPropertyName("nextStartIndex")] int? NextStartIndex,
    [property: JsonPropertyName("items")] List<GenreItemOutputDto> Items);
