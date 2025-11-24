using Asp.Versioning;
using GenericApiProject.Models.GenericModels;
using GenericApiProject.Services.IService;

using Microsoft.AspNetCore.Mvc;

namespace GenericApiProject.Api.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        
        [HttpPost("login")]
        public async Task<ApiResponse> Login(LoginRequest request)
        {
            var response = await _serviceManager.AuthService.Login(request);
            return response;
        }
        
       
    }
}
