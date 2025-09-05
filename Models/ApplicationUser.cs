using Microsoft.AspNetCore.Identity;

namespace ByteInoTaskManager.Models
{
    public class ApplicationUser: IdentityUser
    {

        public bool IsActive { get; set; } = true;
        public string? FullName { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
