using System.ComponentModel.DataAnnotations;

namespace AuthServerDemo.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
