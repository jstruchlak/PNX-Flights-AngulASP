using System.ComponentModel.DataAnnotations;

namespace AngAspPnx.Server.DTO
{
    public record NewPassengerDTO(
        [Required][EmailAddress][StringLength(100, MinimumLength =3)]string Email,
        [Required][MinLength(2)][MaxLength(35)]string FirstName,
        [Required][MinLength(2)][MaxLength(45)] string LastName,
        [Required]bool Gender);
}