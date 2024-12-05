using System.ComponentModel.DataAnnotations;


namespace AngAspPnx.Server.Domain.Entities
{
    public record Booking(
        string PassengerEmail,
        byte NumberOfSeats);
}
