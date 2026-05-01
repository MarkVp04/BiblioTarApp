using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KonyvController : ControllerBase
    {
        private readonly IKonyvServices _konyvServices;

        public KonyvController(IKonyvServices konyvServices)
        {
            _konyvServices = konyvServices;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "AllUserPolicy")]
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
        [Authorize(Policy = "AllUserPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]
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