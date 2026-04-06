using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JellyfinMCP.Routines;
using JellyfinMCP.Configuration;
using JellyfinMCP.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Services.AddSingleton(JellyfinOptions.FromEnvironment());

builder.Services.AddHttpClient<JellyfinApiClient>((sp, client) =>
{
    var options = sp.GetRequiredService<JellyfinOptions>();
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);

    if (!string.IsNullOrWhiteSpace(options.ApiKey))
    {
        client.DefaultRequestHeaders.Add("X-MediaBrowser-Token", options.ApiKey);
    }
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<Images>()
    .WithTools<Actor>()
    .WithTools<Movies>()
    .WithTools<MovieDetails>()
    .WithTools<Libraries>()
    .WithTools<PublicInformation>()
    .WithTools<Genres>();

await builder.Build().RunAsync();
