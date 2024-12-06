using data.Exception;
using remoteData.RemoteDataBase;
using User = data.RemoteData.RemoteDataBase.DAO.UserDao;
using UserDao = data.RemoteData.RemoteDataBase.DAO.UserDao;

namespace data.Repository
{
    public class SQLUserRepositoryImpl : IUserRepository
    {
        private readonly RemoteDatabaseContext _remoteDatabaseContext;

        public SQLUserRepositoryImpl(RemoteDatabaseContext remoteDatabaseContext)
        {
            _remoteDatabaseContext = remoteDatabaseContext;
        }

        
        public IEnumerable<User> GetAllUsers => _remoteDatabaseContext.users
            .Select(u => new UserDao
            {
                Guid = u.Guid,
                FIO = u.FIO,
                GroupID = u.GroupID
            })
            .ToList();

        public bool RemoveUserByGuid(Guid userGuid)
        {
            var user = _remoteDatabaseContext.users.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null) throw new UserNotFoundException(userGuid);

            _remoteDatabaseContext.users.Remove(user);
            _remoteDatabaseContext.SaveChanges(); 
            return true;
        }

        public User? UpdateUser(User user)
        {
            var existingUser = _remoteDatabaseContext.users.FirstOrDefault(u => u.Guid == user.Guid);
            if (existingUser == null) throw new UserNotFoundException(user.Guid);

            existingUser.FIO = user.FIO;
            existingUser.GroupID = user.GroupID;
            _remoteDatabaseContext.SaveChanges(); 

            return new User
            {
                Guid = existingUser.Guid,
                FIO = existingUser.FIO,
                GroupID = existingUser.GroupID
            };
        }

        public IEnumerable<RemoteData.RemoteDataBase.DAO.UserDao> GetAllUsersDao => _remoteDatabaseContext.users.ToList();

        public List<User> GetUserNames()
        {
            return _remoteDatabaseContext.users
                .Select(u => new User { Guid = u.Guid, FIO = u.FIO })
                .ToList();
        }

        List<RemoteData.RemoteDataBase.DAO.UserDao> IUserRepository.GetUserNames()
        {
            throw new NotImplementedException();
        }
    }
}
