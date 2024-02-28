using Microsoft.AspNetCore.Mvc;
using StyleVaulAPI.Dto.CompaniesSetup.Request;
using StyleVaulAPI.Dto.Users.Response;
using StyleVaulAPI.Interfaces.Services;
using StyleVaulAPI.Models.Enums;
using StyleVaulAPI.Models;
using System.Net;
using StyleVaulAPI.Attributes;

namespace StyleVaulAPI.Controllers
{
    [Route("api/companySetupManager")]
    [ApiController]
    [Authorize(RoleEnum.Admin, RoleEnum.Manager, RoleEnum.Team)]
    public class CompaniesSetupsController : ControllerBase
    {
        private readonly ICompaniesSetupService _service;

        public CompaniesSetupsController(ICompaniesSetupService service)
        {
            _service = service;
        }

        [HttpPut]
        public async Task<ActionResult<CompanySetup>> PutCompanySetup(
            CompaniesSetupRequest companySetupRequest
        )
        {
            var companyId = GetCompanyIdOfUser();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.UpdateCompanySetup(companySetupRequest, companyId);

                if (result == false)
                    return BadRequest("Erro ao atualizar configurações!");

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Erro ao atualizar configurações da empressa"
                );
            }
        }

        [HttpGet("/api/collors-company")]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetColorMode()
        {
            try
            {
                var companyId = GetCompanyIdOfUser();
                var result = await _service.GetColorMode(companyId);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Erro ao obter configurações"
                );
            }
        }

        [HttpGet("/api/img-company")]
        [Authorize(RoleEnum.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImgCompany()
        {
            try
            {
                var companyId = GetCompanyIdOfUser();
                var result = await _service.GetImgCompany(companyId);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    "Erro ao obter imagem"
                );
            }
        }

        protected int GetCompanyIdOfUser()
        {
            var user = (UsersResponse)HttpContext.Items["User"]!;
            return user == null ? 0 : user.CompanyId;
        }
    }
}