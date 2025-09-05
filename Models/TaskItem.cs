namespace ByteInoTaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string  Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = TaskStatus.Pending;
        public bool IsCompleted { get; set; } = false;

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
 
}
