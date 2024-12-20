namespace domain.Models
{
    public class Presence
    {

        public required UserDao User { get; set; }
        public required int GroupID { get; set; }
        public bool IsAttedance { get; set; } = true;
        public required DateTime Date { get; set; }

        public required int LessonNumber { get; set; }
    }
}
