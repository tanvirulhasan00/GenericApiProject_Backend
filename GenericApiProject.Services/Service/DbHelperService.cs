using GenericApiProject.Services.IService;
using Microsoft.Extensions.DependencyInjection;

namespace GenericApiProject.Services.Service;

public static class DbHelperService
{
    public static async Task<bool> ChecksDbConnection(IServiceProvider services, string connectionString)
    {
        using var scope = services.CreateScope();
        var dbChecker = scope.ServiceProvider.GetRequiredService<ICheckerService>();

        var isConnected = await dbChecker.IsDbConnectedAsync(connectionString);

        Console.WriteLine(isConnected
            ? "✅ Database is connected!"
            : "❌ Database connection failed. App is shutting down...");

        return isConnected;
    }

    public static async Task SeedDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializerService>();
        await dbInitializer.InitializeAsync();
    }
}