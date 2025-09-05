using System.ComponentModel.DataAnnotations;

namespace ByteInoTaskManager.Models.DTOs
{
    public class RegisterDTO
    {

  

        [Required(ErrorMessage = "Please enter username")]
        [MaxLength(50, ErrorMessage = "maximum 50 char")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [MaxLength(50, ErrorMessage = "maximum 50 char")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Use strong password with more than 4 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords not same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
