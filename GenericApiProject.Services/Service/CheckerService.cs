using GenericApiProject.Services.IService;
using Npgsql;

namespace GenericApiProject.Services.Service;

public class CheckerService : ICheckerService
{
    public async Task<bool> IsDbConnectedAsync(string connectionString)
    {
        try
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            Console.WriteLine("✅ Database is connected!");
            return true;

        }
        catch (Exception ex) 
        {
            Console.WriteLine($"❌ Database connection failed: {ex.Message}");
            return false;
        }
    }
}