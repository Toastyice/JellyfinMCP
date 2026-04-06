using System.ComponentModel;
using System.Text.Json;
using JellyfinMCP.Infrastructure;
using JellyfinMCP.OutputDtos;
using ModelContextProtocol.Server;

namespace JellyfinMCP.Routines;

[McpServerToolType]
public sealed class MovieDetails(JellyfinApiClient jellyfin)
{
    [McpServerTool]
    [Description(
        "Retrieves detailed movie information from Jellyfin by movie ID. " +
        "Returns JSON with overview, genres, production year, studios, people, provider IDs, path, media streams, and community rating.")]
    public async Task<string> GetDetailedMovie(
        [Description("Required: the movie ID")]
        string movieId)
    {
        try
        {
            var movie = await jellyfin.GetMovieDetailsAsync(movieId);

            var payload = new MovieDetailsOutputDto(
                Id: movie.Id,
                Name: movie.Name,
                Type: movie.Type,
                Year: movie.ProductionYear,
                Overview: movie.Overview,
                Genres: movie.Genres,
                Studios: movie.Studios,
                People: movie.People,
                ProviderIds: movie.ProviderIds,
                Path: movie.Path,
                MediaStreams: movie.MediaStreams,
                CommunityRating: movie.CommunityRating);

            return JsonSerializer.Serialize(payload, Infrastructure.JellyfinJsonContext.Default.MovieDetailsOutputDto);
        }
        catch (Exception ex)
        {
            return $"Error connecting to Jellyfin: {ex.Message}";
        }
    }
}
