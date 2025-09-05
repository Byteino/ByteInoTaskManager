using ByteInoTaskManager.Models;

namespace ByteInoTaskManager.Areas.SecretName.Models.ViewModels
{
    public class UserDetailViewModel
    {
        public ApplicationUser  Userd { get; set; }
        public List<Category>? Categories { get; set; }
        public List<TaskItem>? TaskItems { get; set; }
    }
}
