using GenericApiProject.Services.IService;

namespace GenericApiProject.Api;

public static class HelperFunction
{
    public static async Task<bool> ChecksDbConnection(WebApplication app, string connectionString)
    {
        using var scope = app.Services.CreateScope();
        var dbChecker = scope.ServiceProvider.GetRequiredService<ICheckerService>();
        
        var isConnected = await dbChecker.IsDbConnectedAsync(connectionString);

        Console.WriteLine(isConnected
            ? "✅ Database is connected!"
            : "❌ Database connection failed. App is shutting down...");

        return isConnected;
    }

    public static async Task SeedDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializerService>();
        await dbInitializer.InitializeAsync(); 
    }
}