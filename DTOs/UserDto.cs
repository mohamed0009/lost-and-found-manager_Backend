using System.ComponentModel.DataAnnotations;

namespace LostAndFound.API.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
    }
}