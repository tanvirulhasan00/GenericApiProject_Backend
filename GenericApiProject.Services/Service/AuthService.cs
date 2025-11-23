using GenericApiProject.Database.Data;
using GenericApiProject.Models.DatabaseEntity.User;
using GenericApiProject.Models.GenericModels;
using GenericApiProject.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GenericApiProject.Services.Service;

public class AuthService: Service<ApplicationUser>, IAuthService
{
    private readonly GenericApiDbContext _db;

    public AuthService(
        GenericApiDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        string secretKey
    ) : base(db)
    {
        _db = db;
    }
    public async Task<bool> IsUniqueUser(string userName)
    {
        return !await _db.ApplicationUsers.AnyAsync(u => u.UserName == userName);
    }

    public Task<ApiResponse> Login(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public void Update(ApplicationUser user)
    {
        _db.Update(user);
    }
}