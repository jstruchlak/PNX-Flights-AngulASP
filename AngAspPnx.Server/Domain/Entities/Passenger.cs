using System.ComponentModel.DataAnnotations;

namespace AngAspPnx.Server.Domain.Entities
{
    public record Passenger(
        string Email,
        string FirstName,
        string LastName,
        bool Gender);
}