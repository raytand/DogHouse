using Microsoft.AspNetCore.Mvc;

namespace DogHouse.Api.Controllers
{
    [ApiController]
    [Route("/ping")]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _config;
        public PingController(IConfiguration config) => _config = config;

        [HttpGet]
        public IActionResult Get()
        {
            var version = _config["AppSettings:Version"] ?? "Dogshouseservice.Version1.0.1";
            return Ok(version);
        }
    }
}