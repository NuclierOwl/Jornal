namespace data.RemoteData.RemoteDataBase.DAO
{
    public class PresenceDao
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsAttedance { get; set; } = true;
        public DateTime Date {  get; set; }
        public int LessonNumber { get; set; }
        public virtual GroupDao? Group { get; set; }
        public virtual UserDao? User { get; set; }

    }
}
