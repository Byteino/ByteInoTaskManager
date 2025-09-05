using ByteInoTaskManager.Models.DTOs;

namespace ByteInoTaskManager.Models.ViewModel
{
    public class AuthViewModel
    {
        public LoginDTO Login { get; set; }
        public RegisterDTO Register { get; set; }
        public string ActiveForm { get; set; } = "login";

    }
}
