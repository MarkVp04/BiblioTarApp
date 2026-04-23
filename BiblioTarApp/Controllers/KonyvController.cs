using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KonyvController : ControllerBase
    {
        private readonly IKonyvServices _konyvServices;

        public KonyvController(IKonyvServices konyvServices)
        {
            _konyvServices = konyvServices;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] KonyvCreateDto konyvCreateDto)
        {
            try
            {
                var result = await _konyvServices.Create(konyvCreateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _konyvServices.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllKonyv()
        {
            var result = await _konyvServices.List();
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetKonyv(int id)
        {
            var result = await _konyvServices.GetById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] KonyvUpdateDto konyvUpdateDto)
        {
            try
            {
                var result = await _konyvServices.Update(konyvUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}