using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GenericApiProject.Models.DatabaseEntity.User;

public class ApplicationUser : IdentityUser
{
    [MaxLength(20)]
    public string Password { get; set; } = string.Empty;
}