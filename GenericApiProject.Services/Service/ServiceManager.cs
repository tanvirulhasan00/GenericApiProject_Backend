using GenericApiProject.Database.Data;
using GenericApiProject.Models.DatabaseEntity.User;
using GenericApiProject.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GenericApiProject.Services.Service;

public class ServiceManager : IServiceManager
{
    private readonly GenericApiDbContext _db;

    public IAuthService AuthService { get; private set; }


    public ServiceManager(GenericApiDbContext db,IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        var secretKey = configuration.GetSection("SecretKey").Value ?? "";
        AuthService = new AuthService(_db,userManager, roleManager, secretKey);
    }
   
    
    public async Task<int> Save()
    {
      return await _db.SaveChangesAsync();
    }

}