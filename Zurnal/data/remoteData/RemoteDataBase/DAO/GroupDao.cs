namespace data.RemoteData.RemoteDataBase.DAO
{
    public class GroupDao
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual IEnumerable<UserDao> Users { get; set; }
    }
}
