using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class KonyvController : ControllerBase
    {
        private readonly IKonyvServices _konyvServices;

        public KonyvController(IKonyvServices konyvServices)
        {
            _konyvServices = konyvServices;
        }

        [HttpGet]
        public IActionResult List()
        {
            var result = _konyvServices.List();
            return Ok(result);
        }
    }
}
