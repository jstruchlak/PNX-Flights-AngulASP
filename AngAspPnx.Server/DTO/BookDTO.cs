namespace AngAspPnx.Server.DTO
{
    public record BookDTO(Guid FlightId,
        string PassengerEmail,
        byte NumberOfSeats);
}
