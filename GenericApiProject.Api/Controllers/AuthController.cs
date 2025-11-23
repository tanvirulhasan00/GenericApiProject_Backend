using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenericApiProject.Api.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Route("getall")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var res = "tanvir";
            return Ok(res);
        }
    }
}
