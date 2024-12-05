using AngAspPnx.Server.Domain.Errors;
using AngAspPnx.Server.ReadModels;

namespace AngAspPnx.Server.Domain.Entities
{
    public class Flight

    {
        public Guid Id { get; set; }
        public string Airline { get; set; }
        public string Price { get; set; }
        public TimePlace Departure { get; set; }
        public TimePlace Arrival { get; set; }
        public int RemainingNumberOfSeats { get; private set; }
        public int RemaingNumberOfSeat { get; set; }
        public IList<Booking> Bookings = new List<Booking>();

        public Flight()
        {

        }

        public Flight(
        Guid id,
        string airline,
        string price,
        TimePlace departure,
        TimePlace arrival,
        int remaingNumberOfSeats
        )
        {
            Id = id;
            Airline = airline;
            Price = price;
            Departure = departure;
            Arrival = arrival;
            RemainingNumberOfSeats = remaingNumberOfSeats;
        }

        public object? MakeBooking(string passengerEmail, byte numberOfSeats)
        {
            var flight = this;

            if (flight.RemainingNumberOfSeats < numberOfSeats)
            {
                return new OverbookError();
            }


            flight.Bookings.Add(
                new Booking(
                    passengerEmail,
                    numberOfSeats
                ));

            flight.RemainingNumberOfSeats -= numberOfSeats;
            return null;
        }
    }

}
