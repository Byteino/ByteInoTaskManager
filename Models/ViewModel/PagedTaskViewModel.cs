using ByteInoTaskManager.Models.DTOs;

namespace ByteInoTaskManager.Models.ViewModel
{
    public class PagedTaskViewModel
    {
        public List<TaskItemDTO> Tasks{ get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
