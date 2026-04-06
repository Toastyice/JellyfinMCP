using System.Text.Json.Serialization;
using JellyfinMCP.OutputDtos;

namespace JellyfinMCP.Infrastructure;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(JellyfinSearchResponse))]
[JsonSerializable(typeof(JellyfinGenreResponse))]
[JsonSerializable(typeof(JellyfinLibraryResponse))]
[JsonSerializable(typeof(JellyfinPublicInfo))]
[JsonSerializable(typeof(JellyfinMovieDetails))]
[JsonSerializable(typeof(List<JellyfinImageInfo>))]
[JsonSerializable(typeof(JellyfinPersonsResponse))]
[JsonSerializable(typeof(ImageResponseDto))]
[JsonSerializable(typeof(ImageItemDto))]
[JsonSerializable(typeof(ErrorDto))]
[JsonSerializable(typeof(InternalErrorDto))]
[JsonSerializable(typeof(PersonResponseDto))]
[JsonSerializable(typeof(PersonItemOutputDto))]
[JsonSerializable(typeof(MovieResponseDto))]
[JsonSerializable(typeof(MovieItemOutputDto))]
[JsonSerializable(typeof(MovieDetailsOutputDto))]
[JsonSerializable(typeof(LibraryResponseDto))]
[JsonSerializable(typeof(LibraryItemOutputDto))]
[JsonSerializable(typeof(PublicInfoOutputDto))]
[JsonSerializable(typeof(GenreResponseDto))]
[JsonSerializable(typeof(GenreItemOutputDto))]
[JsonSerializable(typeof(System.Net.HttpStatusCode))]
public sealed partial class JellyfinJsonContext : JsonSerializerContext;
