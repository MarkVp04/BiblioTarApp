using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LakcimController : ControllerBase
    {
        private readonly ILakcimService _lakcimService;

        public LakcimController(ILakcimService lakcimService)
        {
            _lakcimService = lakcimService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] LakcimCreateDto lakcimCreateDto)
        {
            var result = await _lakcimService.CreateAsyncLakcim(lakcimCreateDto);
            return Ok(result);
        }
    }
}