using System.ComponentModel.DataAnnotations;

namespace AuthServerDemo.Models.UserProfile
{
    public class UserRegisterModel : UserProfileModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
