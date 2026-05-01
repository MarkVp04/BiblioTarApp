using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoglalasController : ControllerBase
    {
        private readonly IFoglalasService _foglalasService;

        public FoglalasController(IFoglalasService foglalasService)
        {
            _foglalasService = foglalasService;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> Create([FromBody] FoglalasCreateDto foglalasCreateDto)
        {
            try
            {
                var result = await _foglalasService.Create(foglalasCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getall")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _foglalasService.List();

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _foglalasService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{felhasznaloId}")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> GetByFelhasznaloId(int felhasznaloId)
        {
            try
            {
                var result = await _foglalasService.GetByFelhasznaloId(felhasznaloId);

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
        [Route("status")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> UpdateStatus([FromBody] FoglalasStatuszUpdateDto foglalasStatuszUpdateDto)
        {
            try
            {
                var result = await _foglalasService.UpdateStatus(foglalasStatuszUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("cancel/{id}")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var result = await _foglalasService.Cancel(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}