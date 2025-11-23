using GenericApiProject.Models.DatabaseEntity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GenericApiProject.Database.Data;

public class GenericApiDbContext : IdentityDbContext<ApplicationUser>
{
    public GenericApiDbContext(DbContextOptions<GenericApiDbContext> options) : base(options)
    {
        
    }
    //db table
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}