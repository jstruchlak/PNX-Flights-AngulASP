using AngAspPnx.Server.ReadModels;
using Microsoft.AspNetCore.Mvc;
using AngAspPnx.Server.DTO;
using AngAspPnx.Server.Domain.Entities;
using AngAspPnx.Server.Domain.Errors;
using AngAspPnx.Server.Data;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        
        private readonly ILogger<FlightController> _logger;

        private readonly Entities _entities;


        public FlightController(ILogger<FlightController> logger,
            Entities entities)
        {
            _logger = logger;
            _entities = entities;
        }

        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Flight>), 200)]
        [HttpGet]
        public IEnumerable<FlightRm> Search()
        {
            var flightRmList = _entities.Flights.Select(flight => new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.RemainingNumberOfSeats
                )).ToArray();

            return flightRmList;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm), 200)]
        [HttpGet("{id}")]
        public ActionResult<FlightRm> Find(Guid id)
        {
            var flight = _entities.Flights.SingleOrDefault(f => f.Id == id); 
            
            if (flight == null)
                return NotFound();

            var readModel = new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(),flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(),flight.Arrival.Time),
                flight.RemainingNumberOfSeats
                );

            return Ok(readModel);
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

                var flight = _entities.Flights.SingleOrDefault(f => f.Id == dto.FlightId);

                if (flight == null)
                    return NotFound($"Flight with ID {dto.FlightId} not found.");

                var error =  flight.MakeBooking(dto.PassengerEmail, dto.NumberOfSeats);

                if (error is OverbookError)
                    return Conflict(new { message = "Not enough seats" });
                _entities.SaveChanges();

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








