using User = data.RemoteData.RemoteDataBase.DAO.UserDao;

namespace data.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers { get; }
        bool RemoveUserByGuid(Guid userGuid);
        User? UpdateUser(User user);
        List<User> GetUserNames();


    }
}
