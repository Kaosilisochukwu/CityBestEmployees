using System.ComponentModel.DataAnnotations;

namespace CityBestEmployees.MS.DTOs
{
    public class SignInDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
