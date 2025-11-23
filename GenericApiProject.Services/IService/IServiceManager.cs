namespace GenericApiProject.Services.IService;

public interface IServiceManager
{
    Task<int> Save();
    
    // service registration
    public IAuthService AuthService { get; }
}