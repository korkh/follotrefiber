using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(
            "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$",
            ErrorMessage = "Password must be complex with at least one uppercase letter, one lowercase letter, and either a digit or one of the following special characters: @, #, $, %, ^, & or +"
        )]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
