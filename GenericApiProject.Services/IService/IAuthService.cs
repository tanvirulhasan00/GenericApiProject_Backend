using GenericApiProject.Models.DatabaseEntity.User;
using GenericApiProject.Models.GenericModels;

namespace GenericApiProject.Services.IService;

public interface IAuthService : IService<ApplicationUser>
{
    Task<bool> IsUniqueUser(string userName);
    Task<ApiResponse> Login(LoginRequest request);
    void Update(ApplicationUser user);
}