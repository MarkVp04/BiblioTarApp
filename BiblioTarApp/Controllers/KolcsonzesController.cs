using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KolcsonzesController : ControllerBase
    {
        private readonly IKolcsonzesService _kolcsonzesService;

        public KolcsonzesController(IKolcsonzesService kolcsonzesService)
        {
            _kolcsonzesService = kolcsonzesService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] KolcsonzesCreateDto kolcsonzesCreateDto)
        {
            try
            {
                var result = await _kolcsonzesService.Create(kolcsonzesCreateDto);
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
            var result = await _kolcsonzesService.List();

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
                var result = await _kolcsonzesService.GetById(id);
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
                var result = await _kolcsonzesService.GetByFelhasznaloId(felhasznaloId);

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
        [Route("extend")]
        public async Task<IActionResult> Hosszabbitas([FromBody] KolcsonzesHosszabbitasDto kolcsonzesHosszabbitasDto)
        {
            try
            {
                var result = await _kolcsonzesService.Hosszabbitas(kolcsonzesHosszabbitasDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] KolcsonzesStatuszUpdateDto kolcsonzesStatuszUpdateDto)
        {
            try
            {
                var result = await _kolcsonzesService.UpdateStatus(kolcsonzesStatuszUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}