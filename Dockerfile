# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0-noble-aot AS build
WORKDIR /src

# Copy project file first for better layer caching
COPY *.csproj ./
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore

# Copy the remaining source
COPY . .

# Publish with Native AOT enabled
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/src/bin \
    --mount=type=cache,target=/src/obj \
    dotnet publish -c Release -r linux-x64 \
        -p:PublishAot=true \
        -p:InvariantGlobalization=true \
        -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled AS runtime
WORKDIR /app

COPY --from=build /app/publish/JellyfinMCP ./

# Run as non-root
USER app

ENTRYPOINT ["./JellyfinMCP"]
