namespace data.RemoteData.RemoteDataBase.DAO
{
    public class UserDao
    {
        public  string FIO { get; set; }
        public Guid Guid { get; set; }

        public  int GroupID { get; set; }

        public virtual GroupDao Group { get; set; }
        public virtual IEnumerable<PresenceDao> Presences{ get; set; }
    }
}