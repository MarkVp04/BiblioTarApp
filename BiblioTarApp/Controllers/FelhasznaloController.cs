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
    public class FelhasznaloController : ControllerBase
    {
        private readonly IFelhasznaloService _felhasznaloService;

        public FelhasznaloController(IFelhasznaloService felhasznaloService)
        {
            _felhasznaloService = felhasznaloService;
        }
        
        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [Authorize(Policy = "StaffPolicy")]
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
        [Authorize(Policy = "StaffPolicy")]
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
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> Update([FromBody] FelhasznaloUpdateDto felhasznaloUpdateDto)
        {
            felhasznaloUpdateDto.Id = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
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
        [Route("delete")]
        [Authorize(Policy = "AllUserPolicy")]
        public async Task<IActionResult> SoftDelete()
        {
            try
            {
                int id = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
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
        [Authorize(Policy = "AdminPolicy")]
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