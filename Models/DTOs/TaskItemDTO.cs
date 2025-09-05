using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ByteInoTaskManager.Models.DTOs
{
    public class TaskItemDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }=string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Status { get; set; } = Models.TaskStatus.Pending;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
