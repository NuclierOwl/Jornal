using data.Exception;
using data.RemoteData.RemoteDataBase.DAO;
using User = data.RemoteData.RemoteDataBase.DAO.UserDao;

namespace data.Repository
{
    public class UserRepositoryImpl : IUserRepository
    {
        public List<User> _users;

        public IEnumerable<User> GetAllUsers => throw new NotImplementedException();

        public UserRepositoryImpl()
        {
            _users = new List<User>();
        }

        public bool RemoveUserByGuid(Guid userGuid)  
        {
            var user = _users.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null) throw new UserNotFoundException(userGuid);

            _users.Remove(user);
            return true;
        }

        public User? UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Guid == user.Guid);
            if (existingUser == null) throw new UserNotFoundException(user.Guid);

            existingUser.FIO = user.FIO;
            existingUser.GroupID = user.GroupID;

            return existingUser;
        }

        public List<User> GetUserNames()
        {
            return _users
                .Select(u => new User
                {
                    Guid = u.Guid,
                    FIO = u.FIO
                })
                .ToList();
        }

        List<UserDao> IUserRepository.GetUserNames()
        {
            throw new NotImplementedException();
        }

        public User? AddUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
