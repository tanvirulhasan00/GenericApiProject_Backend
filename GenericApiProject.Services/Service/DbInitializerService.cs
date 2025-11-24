using GenericApiProject.Database.Data;
using GenericApiProject.Models.DatabaseEntity.User;
using GenericApiProject.Services.IService;
using GenericApiProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GenericApiProject.Services.Service;

public class DbInitializerService : IDbInitializerService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly GenericApiDbContext _dbContext;
    
    public DbInitializerService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, GenericApiDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }
    
    public async Task InitializeAsync()
        {
            await ApplyMigrationsAsync();
            await SeedRolesAsync();
            await SeedAdminUserAsync();
        }

        private async Task ApplyMigrationsAsync()
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
            else
            {
                Console.WriteLine("ℹ️ No pending migrations.");
            }
        }
        private async Task SeedRolesAsync()
        {
            string[] roles = { RoleVariable.Admin, RoleVariable.User };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            const string adminEmail = "admin@gmail.com";
            const string adminPassword = "aDmin@00#";

            var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    PhoneNumber = "01970806028",
                    Password = adminPassword,
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, RoleVariable.Admin);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
}