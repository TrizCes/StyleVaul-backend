using Microsoft.AspNetCore.Mvc;
using StyleVaulAPI.Attributes;
using StyleVaulAPI.Dto.Collections.Request;
using StyleVaulAPI.Dto.Users.Response;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models.Enums;
using System.Net;
namespace StyleVaulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(RoleEnum.Admin, RoleEnum.Manager, RoleEnum.Team)]
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionsService _service;

        public CollectionsController(ICollectionsService collectionService)
        {
            _service = collectionService;
        }

        [HttpGet]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListCollections([FromQuery] int? limit = 20, [FromQuery] int? page = 1)
        {
            try
            {
                if ((!limit.HasValue || limit.Value <= 0) && (!page.HasValue || page.Value <= 0))
                {
                    return BadRequest(
                        "Pelo menos um dos parâmetros 'limit' ou 'page' deve ser fornecido e ser maior que zero."
                    );
                }

                var collections = await _service.GetAllAsync(GetCompanyIdOfUser());

                var pageSize = limit.GetValueOrDefault(20);
                var pageNumber = page.GetValueOrDefault(1);
                var skipAmount = (pageNumber - 1) * pageSize;

                if (skipAmount < 0)
                {
                    skipAmount = 0;
                }

                return Ok(collections.Skip(skipAmount).Take(pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListCollectionById([FromRoute] int id)
        {
            return Ok(await _service.GetByIdAsync(id, GetCompanyIdOfUser()));
        }

        [HttpGet("maiores-orcamentos/limit")]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExpensiveBudgets([FromQuery] int? limit = 6)
        {
            try
            {
                var companyId = GetCompanyIdOfUser();

                var result = await _service.GetExpensiveBudgetsAsync(
                    companyId,
                    limit.Value
                );

                if (result == null)
                    return NotFound();


                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Erro ao buscar coleções"
                );
            }
        }

        [HttpGet("orcamento-vs-custo")]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBudgetsVsCosts()
        {
            try
            {
                var companyId = GetCompanyIdOfUser();

                var result = await _service.GetBudgetsVsCosts(companyId);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Erro ao obter relacionamento de orçamentos e custos das coleções"
                );
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCollection([FromBody] PostCollectionsDto collectionDto)
        {
            return CreatedAtAction(nameof(CreateCollection), new { id = await _service.CreateAsync(collectionDto, GetCompanyIdOfUser()) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCollection([FromRoute] int id, [FromBody] PutCollectionsDto collectionDto)
        {
            return Ok(new { id, collection = await _service.UpdateAsync(collectionDto.SetCollectionId(id), GetCompanyIdOfUser()) });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection([FromRoute] int id)
        {
            await _service.DeleteAsync(id, GetCompanyIdOfUser());
            return NoContent();
        }

        protected int GetCompanyIdOfUser()
        {
            var user = (UsersResponse)HttpContext.Items["User"]!;
            return user.CompanyId;
        }
    }
}

