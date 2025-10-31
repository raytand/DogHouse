using Microsoft.AspNetCore.Mvc;
using DogHouse.Api.Services;
using DogHouse.Api.DTOs;
using AutoMapper;

namespace DogHouse.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class DogsController : ControllerBase
    {
        private readonly IDogService _service;
        private readonly IMapper _mapper;

        public DogsController(IDogService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] string? attribute, [FromQuery] string? order,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            try
            {
                var (dogs, total) = await _service.GetDogsAsync(attribute, order, pageNumber, pageSize);
                var dto = _mapper.Map<List<DTOs.DogDto>>(dogs);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] CreateDogDto createDto)
        {
            if (createDto == null)
                return BadRequest(new { error = "Invalid JSON" });

            try
            {
                await _service.CreateDogAsync(createDto);
                return CreatedAtAction(nameof(GetDogs), new { name = createDto.Name }, null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}