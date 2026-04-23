using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuntetesController : ControllerBase
    {
        private readonly IBuntetesService _buntetesService;

        public BuntetesController(IBuntetesService buntetesService)
        {
            _buntetesService = buntetesService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] BuntetesCreateDto buntetesCreateDto)
        {
            try
            {
                var result = await _buntetesService.Create(buntetesCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _buntetesService.List();

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _buntetesService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{felhasznaloId}")]
        public async Task<IActionResult> GetByFelhasznaloId(int felhasznaloId)
        {
            try
            {
                var result = await _buntetesService.GetByFelhasznaloId(felhasznaloId);

                if (result == null || !result.Any())
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("payment")]
        public async Task<IActionResult> UpdateFizetesiStatusz([FromBody] BuntetesFizetesiStatuszUpdateDto buntetesFizetesiStatuszUpdateDto)
        {
            try
            {
                var result = await _buntetesService.UpdateFizetesiStatusz(buntetesFizetesiStatuszUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}