namespace MinimalBackend;

public static class Endpoints
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
    }
}