using BiblioTarApp.DTOs;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "StaffPolicy")]
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
        [Authorize(Policy = "StaffPolicy")]
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
        [Authorize(Policy = "StaffPolicy")]
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
        [Authorize(Policy = "AllUserPolicy")]
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

        // Ezt a felhasználói felület hívja, amikor a user hosszabbítást kér.
        // Nem hosszabbít azonnal, csak "hosszabbításra vár" állapotba teszi.
        [HttpPut]
        [Route("request-extension/{id}")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> RequestExtension(int id)
        {
            try
            {
                var result = await _kolcsonzesService.RequestExtension(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Ezt a könyvtáros használja a hosszabbítás engedélyezésére.
        // Itt történik ténylegesen a határidő módosítása.
        [HttpPut]
        [Route("extend")]
        [Authorize(Policy = "StaffPolicy")]
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
        [Authorize(Policy = "StaffPolicy")]
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

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Policy = "StaffPolicy")]
        public async Task<IActionResult> DeleteClosedLoan(int id)
        {
            try
            {
                var result = await _kolcsonzesService.DeleteClosedLoan(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
