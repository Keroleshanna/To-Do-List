namespace To_Do.Models
{
    public class OwnToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
