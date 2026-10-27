using System.ComponentModel.DataAnnotations;

namespace myFlightPerformance.Models;

public class LoginRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class CreateInstructorRequest
{
    [Required, StringLength(64, MinimumLength = 3)]
    public required string Username { get; set; }

    [Required, StringLength(128, MinimumLength = 12)]
    public required string Password { get; set; }
}
