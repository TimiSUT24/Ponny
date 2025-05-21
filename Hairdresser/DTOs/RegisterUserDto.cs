using System.ComponentModel.DataAnnotations;

namespace Hairdresser.DTOs
{
    public class RegisterUserDto
    {
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
