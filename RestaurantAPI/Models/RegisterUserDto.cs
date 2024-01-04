using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class RegisterUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
		public string Password { get; set; }
        public string Nationality{ get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int RoleId { get; set; } = 1;
    }
}