using AngAspPnx.Server.ReadModels;
using Microsoft.AspNetCore.Mvc;
using AngAspPnx.Server.DTO;


namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;

        static Random random = new Random();

        static private IList<BookDTO> Bookings = new List<BookDTO>();


        static private FlightRm[] flights = new FlightRm[]



            {
        new (   Guid.NewGuid(),
                "American Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Los Angeles",DateTime.Now.AddHours(random.Next(1, 3))),
                new TimePlaceRm("Istanbul",DateTime.Now.AddHours(random.Next(4, 10))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Deutsche BA",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Munchen",DateTime.Now.AddHours(random.Next(1, 10))),
                new TimePlaceRm("Schiphol",DateTime.Now.AddHours(random.Next(4, 15))),
                random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "British Airways",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("London, England",DateTime.Now.AddHours(random.Next(1, 15))),
                new TimePlaceRm("Vizzola-Ticino",DateTime.Now.AddHours(random.Next(4, 18))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Basiq Air",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Amsterdam",DateTime.Now.AddHours(random.Next(1, 21))),
                new TimePlaceRm("Glasgow, Scotland",DateTime.Now.AddHours(random.Next(4, 21))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "BB Heliag",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Zurich",DateTime.Now.AddHours(random.Next(1, 23))),
                new TimePlaceRm("Baku",DateTime.Now.AddHours(random.Next(4, 25))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Adria Airways",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Ljubljana",DateTime.Now.AddHours(random.Next(1, 15))),
                new TimePlaceRm("Warsaw",DateTime.Now.AddHours(random.Next(4, 19))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "ABA Air",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Praha Ruzyne",DateTime.Now.AddHours(random.Next(1, 55))),
                new TimePlaceRm("Paris",DateTime.Now.AddHours(random.Next(4, 58))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "AB Corporate Aviation",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Le Bourget",DateTime.Now.AddHours(random.Next(1, 58))),
                new TimePlaceRm("Zagreb",DateTime.Now.AddHours(random.Next(4, 60))),
                    random.Next(1, 853))
            };

        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }

        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
        [HttpGet]
        public IEnumerable<FlightRm> Search()
        => flights;

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm), 200)]
        [HttpGet("{id}")]
        public ActionResult<FlightRm> Find(Guid id)
        {
            var flight = flights.SingleOrDefault(f => f.Id == id); 
            
            if (flight == null)
                return NotFound();

            return Ok(flight);
        }

        /* Swagger API Docs*/
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(201)]
        public IActionResult Book(BookDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid booking data.");
                }

                System.Diagnostics.Debug.WriteLine($"Booking a new flight {dto.FlightId}");

                var flightFound = flights.Any(f => f.Id == dto.FlightId);

                if (!flightFound)
                    return NotFound($"Flight with ID {dto.FlightId} not found.");

                Bookings.Add(dto);

                // Ensure CreatedAtAction references a valid action with matching route parameters.
                return CreatedAtAction(nameof(Find), new { id = dto.FlightId }, dto);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



    }
}








