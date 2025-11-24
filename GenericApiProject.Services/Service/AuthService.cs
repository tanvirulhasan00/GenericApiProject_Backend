using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using GenericApiProject.Database.Data;
using GenericApiProject.Models.DatabaseEntity.User;
using GenericApiProject.Models.GenericModels;
using GenericApiProject.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GenericApiProject.Services.Service;

public class AuthService: Service<ApplicationUser>, IAuthService
{
    private readonly GenericApiDbContext _db;
    private readonly ApiResponse _response;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly RoleManager<IdentityRole> _roleManager;
    private readonly string _secretKey;

    public AuthService(
        GenericApiDbContext db,
        UserManager<ApplicationUser> userManager,
        string secretKey
    ) : base(db)
    {
        _db = db;
         _response = new ApiResponse();
         _userManager = userManager;
         
         _secretKey = secretKey;
    }
    public async Task<bool> IsUniqueUser(string userName)
    {
        return !await _db.ApplicationUsers.AnyAsync(u => u.UserName == userName);
    }

    public async Task<ApiResponse> Login(LoginRequest request)
    {
        var loginResponse = new LoginResponse();
        try
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                _response.Success = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "Username or password is incorrect.";
                return _response;
            }
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user == null)
            {
                _response.Success = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "Username or password is incorrect.";
                return _response;
            }
            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValid)
            {
                _response.Success = false;
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.Message = "Username or password is incorrect.";
                return _response;
            }
            
            //roles 
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenExpire = request.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddMinutes(30);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
                ]),
                Expires = tokenExpire,
                SigningCredentials =  new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            
            loginResponse.UserId = user.Id;
            loginResponse.Role = roles.FirstOrDefault() ?? string.Empty;
            loginResponse.Token = tokenHandler.WriteToken(token);
            loginResponse.TokenExpire = tokenExpire;
            
            _response.Success = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Login successful.";
            _response.Result = loginResponse;
            return _response;
        }catch(Exception e)
        {
            _response.Success = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.Message = e.Message;
            return _response;
        }
    }

    public void Update(ApplicationUser user)
    {
        _db.Update(user);
    }
}