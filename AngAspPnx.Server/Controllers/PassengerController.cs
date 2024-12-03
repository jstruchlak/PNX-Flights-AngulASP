using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AngAspPnx.Server.DTO;
using AngAspPnx.Server.ReadModels;

namespace AngAspPnx.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        static private IList<NewPassengerDTO> Passengers = new List<NewPassengerDTO>();
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Register(NewPassengerDTO dto)
        {
            Passengers.Add(dto);
            System.Diagnostics.Debug.WriteLine(Passengers.Count);
            var locationUrl = Url.Link(nameof(Find), new { email = dto.Email });
            System.Diagnostics.Debug.WriteLine($"Location URL: {locationUrl}");

            return CreatedAtAction(nameof(Find), new { email = dto.Email }, dto);
        }
        [HttpGet("{email}")]
        public ActionResult<PassengerRm> Find(string email)
        {
            var passenger = Passengers.FirstOrDefault(p => p.Email == email);
            if (passenger == null)
                return NotFound();
            var rm = new PassengerRm(
                passenger.Email,
                passenger.FirstName,
                passenger.LastName,
                passenger.Gender
                );
            return Ok(rm);
        }
    }
}
