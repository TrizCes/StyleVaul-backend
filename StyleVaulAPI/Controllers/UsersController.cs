using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StyleVaulAPI.Dto.Users.Request;
using StyleVaulAPI.Dto.Users.Response;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models;
using System.Net;

namespace StyleVaulAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;
        private readonly IMapper _mapper;

        public UsersController(IUsersService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostUsers user)
        {
            var changer = (UsersResponse)HttpContext.Items["User"]!;
            user.CompanyId = changer.CompanyId;
            var result = await _service.CreateAsync(_mapper.Map<User>(user));

            return Created("users", result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PutUsers user)
        {
            var changer = (UsersResponse)HttpContext.Items["User"]!;
            return Ok(await _service.UpdateAsync(id, user, changer));
        }

        [HttpPost("password_reset")]
        [AllowAnonymous]
        public async Task<IActionResult> PostPasswordReset([FromBody] PostPasswordReset postPasswordReset)
        {
            try
            {
                var result = await _service.GetEmailAsync(postPasswordReset.Email);

                if (result == false)
                    return NotFound();

                return Ok(new { result });
            }
            catch (Exception e)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Erro ao solicitar redefinição de senha."
                );
            }
        }

        [HttpPatch("profile-update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchPassword(
            [FromBody] PasswordConfirmationRequest passwordConfirmation
        )
        {
            return Ok(new { success = await _service.PatchPaswordAsync(GetUserIdOfUser(), passwordConfirmation), message = "Senha alterada com sucesso" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync(GetCompanyIdOfUser()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            await _service.DeleteAsync(id, GetUserIdOfUser());
            return NoContent();
        }

        protected int GetUserIdOfUser()
        {
            var user = (UsersResponse)HttpContext.Items["User"]!;
            return user.Id;
        }

        protected int GetCompanyIdOfUser()
        {
            var user = (UsersResponse)HttpContext.Items["User"]!;
            return user.CompanyId;
        }
    }
}