namespace GenericApiProject.Services.IService;

public interface ICheckerService
{
    Task<bool> IsDbConnectedAsync(string connectionString);
}