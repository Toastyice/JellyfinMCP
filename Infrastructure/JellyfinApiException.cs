namespace JellyfinMCP.Infrastructure;

public sealed class JellyfinApiException : Exception
{
    public System.Net.HttpStatusCode? StatusCode { get; }

    public string? ResponseBody { get; }

    public JellyfinApiException(string message)
        : base(message)
    {
    }

    public JellyfinApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public JellyfinApiException(string message, System.Net.HttpStatusCode? statusCode, string? responseBody)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public override string ToString()
    {
        var status = StatusCode is null ? string.Empty : $"{Environment.NewLine}StatusCode: {StatusCode}";
        var body = string.IsNullOrWhiteSpace(ResponseBody) ? string.Empty : $"{Environment.NewLine}ResponseBody: {ResponseBody}";
        return $"{base.ToString()}{status}{body}";
    }
}