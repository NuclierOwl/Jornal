namespace data.RemoteData.RemoteDataBase.DAO
{
    public class Excel
    {
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public bool IsAttedance { get; set; }
        public int LessonNumber { get; set; }
        public string GroupName { get; set; }
    }
}