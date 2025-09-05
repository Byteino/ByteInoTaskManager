using System.ComponentModel.DataAnnotations;

namespace ByteInoTaskManager.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter Email")]
        [Display(Name ="Email")]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Use strong password with more than 4 characters.")]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
