using Microsoft.AspNetCore.Mvc;
using StyleVaulAPI.Attributes;
using StyleVaulAPI.Dto.Models.Request;
using StyleVaulAPI.Dto.Users.Response;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models.Enums;

namespace StyleVaulAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(RoleEnum.Manager, RoleEnum.Team, RoleEnum.Admin)]
    public class ModelsController : ControllerBase
    {
        private readonly IModelsService _modelService;

        public ModelsController(IModelsService service)
        {
            _modelService = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel([FromBody] PostModels model)
        {
            var loggedUser = (UsersResponse)HttpContext.Items["User"]!;
            var result = await _modelService.CreateAsync(model, loggedUser.CompanyId);
            return CreatedAtAction(nameof(GetModelById), new { id = result }, model);
        }

        [HttpGet]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListModels([FromQuery] int? limit = 20, [FromQuery] int? page = 1)
        {
            var loggedUser = (UsersResponse)HttpContext.Items["User"]!;
            var query = await _modelService.GetAllAsync(loggedUser.CompanyId);

            if (!query.Any())
            {
                return NotFound("Nenhum modelo encontrado.");
            }

            var pageSize = limit.GetValueOrDefault(20);
            var pageNumber = page.GetValueOrDefault(1);
            var skipAmount = (pageNumber - 1) * pageSize;

            if (skipAmount < 0)
            {
                skipAmount = 0;
            }

            return Ok(query.Skip(skipAmount).Take(pageSize));
        }

        [HttpGet("{id}")]
        [Authorize(RoleEnum.ReadOnly)]
        public async Task<IActionResult> GetModelById(int id)
        {
            var loggedUser = (UsersResponse)HttpContext.Items["User"]!;
            return Ok(await _modelService.GetByIdAsync(id, loggedUser.CompanyId));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateModel([FromRoute] int id, [FromBody] PutModels modelDto)
        {
            var loggedUser = (UsersResponse)HttpContext.Items["User"]!;
            var updatedModel = await _modelService.UpdateAsync(modelDto, id, loggedUser.CompanyId);

            return Ok(updatedModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelById(int id)
        {
            var loggedUser = (UsersResponse)HttpContext.Items["User"]!;
            await _modelService.DeleteAsync(id, loggedUser.CompanyId);
            return NoContent();
        }
    }
}