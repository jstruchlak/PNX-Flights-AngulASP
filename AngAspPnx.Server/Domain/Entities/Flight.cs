using AngAspPnx.Server.Domain.Errors;
using AngAspPnx.Server.ReadModels;

namespace AngAspPnx.Server.Domain.Entities
{
    public record Flight(
        Guid Id,
        string Airline,
        string Price,
        TimePlace Departure,
        TimePlace Arrival,
        int RemaingNumberOfSeats
        )

    {
        public IList<Booking> Bookings = new List<Booking>();
        public int RemaingNumberOfSeats { get; set; } = RemaingNumberOfSeats;

        public object? MakeBooking(string passengerEmail, byte numberOfSeats)
        {
            var flight = this;

            if (flight.RemaingNumberOfSeats < numberOfSeats)
            {
                return new OverbookError();
            }


            flight.Bookings.Add(
                new Booking(
                    passengerEmail,
                    numberOfSeats
                ));

            flight.RemaingNumberOfSeats -= numberOfSeats;
            return null;
        }
    }

}
