using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FelhasznaloController : ControllerBase
    {
        private readonly IFelhasznaloService _felhasznaloService;

        public FelhasznaloController(IFelhasznaloService felhasznaloService)
        {
            _felhasznaloService = felhasznaloService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] FelhasznaloCreateDto felhasznaloCreateDto)
        {
            try
            {
                var result = await _felhasznaloService.Create(felhasznaloCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] FelhasznaloLoginDto felhasznaloLoginDto)
        {
            try
            {
                var result = await _felhasznaloService.Login(felhasznaloLoginDto);
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
            var result = await _felhasznaloService.List();

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
                var result = await _felhasznaloService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] FelhasznaloUpdateDto felhasznaloUpdateDto)
        {
            try
            {
                var result = await _felhasznaloService.Update(felhasznaloUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var result = await _felhasznaloService.SoftDelete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update-role")]
        public async Task<IActionResult> UpdateSzerepkor([FromBody] FelhasznaloSzerepkorUpdateDto felhasznaloSzerepkorUpdateDto)
        {
            try
            {
                var result = await _felhasznaloService.UpdateSzerepkor(felhasznaloSzerepkorUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}