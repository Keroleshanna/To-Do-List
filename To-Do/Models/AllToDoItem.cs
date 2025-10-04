namespace To_Do.Models
{
    public class AllToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly Deedline { get; set; }
        public string? File { get; set; }

        public int OwnToDoListId { get; set; }
        public OwnToDoList OwnToDoList { get; set; }
    }
}
