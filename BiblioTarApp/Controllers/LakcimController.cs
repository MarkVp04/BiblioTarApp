using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LakcimController : ControllerBase
    {
        private readonly ILakcimService _lakcimService;

        public LakcimController(ILakcimService lakcimService)
        {
            _lakcimService = lakcimService;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> Create([FromBody] LakcimCreateDto request)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                var lakcimCreateDto = new LakcimCreateDto
                {
                    Iranyitoszam = request.Iranyitoszam,
                    Varos = request.Varos,
                    Utca = request.Utca,
                    Hazszam = request.Hazszam,
                    FelhasznaloId = userId
                };

                var result = await _lakcimService.CreateAsyncLakcim(lakcimCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("save-current")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> SaveCurrent([FromBody] LakcimCreateDto request)
        {
            try
            {
                int userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                var lakcimCreateDto = new LakcimCreateDto
                {
                    Iranyitoszam = request.Iranyitoszam,
                    Varos = request.Varos,
                    Utca = request.Utca,
                    Hazszam = request.Hazszam,
                    FelhasznaloId = userId
                };

                var result = await _lakcimService.CreateOrUpdateSingleAsync(lakcimCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}