using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace ByteInoTaskManager.Areas.SecretName.Models.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }

        [Display(Name = "User Email")]
        public string Email { get; set; }
        public bool IsActive { get; set; }

        [Display(Name = "Task Count")]
        public int TaskCount { get; set; }

        [Display(Name = "Category Count")]
        public int CategoryCount { get; set; }

    }
}
