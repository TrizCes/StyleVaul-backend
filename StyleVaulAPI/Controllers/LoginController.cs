using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Interfaces.Services;

namespace StyleVaulAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsersService _service;

        public LoginController(IUsersService service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateJwtToken(AuthenticateRequest request)
        {

            return Ok(new { token = await _service.GenerateJwtToken(request) });
        }
    }
}
