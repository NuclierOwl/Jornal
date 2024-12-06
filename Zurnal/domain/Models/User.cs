namespace domain.Models
{
    public class UserDao
    {
        public required string FIO { get; set; }
        public Guid Guid { get; set; }
        public required Group Group { get; set; }
    }
}
