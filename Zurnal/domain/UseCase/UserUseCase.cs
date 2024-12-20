using data.Exception;
using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;

namespace domain.UseCase
{
    public class UserUseCase
    {
        public readonly IUserRepository _repositoryUserImpl;
        public readonly IGroupRepository _repositoryGroupImpl;

        public UserUseCase(IUserRepository repositoryImpl, IGroupRepository repositoryGroupImpl)
        {
            _repositoryUserImpl = repositoryImpl;
            _repositoryGroupImpl = repositoryGroupImpl;
        }

        public List<UserDao> GetAllUsers() => _repositoryUserImpl.GetAllUsers
            .Join(_repositoryGroupImpl.GetAllGroup(),
            user => user.GroupID,
            group => group.Id,
            (user, group) =>
            new UserDao
            {
                FIO = user.FIO,
                Guid = user.Guid,
                Group = new GroupDao { Id = group.Id, Name = group.Name }
            }).ToList();

        public bool RemoveUserByGuid(Guid userGuid)
        {
            try
            {
                return _repositoryUserImpl.RemoveUserByGuid(userGuid);
            }
            catch (UserNotFoundException)
            {
                return false;
            }
            catch (RepositoryException)
            {
                return false;
            }
        }


        public UserDao UpdateUser(UserDao user)
        {
            UserDao userLocalEnity = new UserDao
            {
                FIO = user.FIO,
                Guid = user.Guid,
                GroupID = user.GroupID,
                Group = _repositoryGroupImpl.GetAllGroup().FirstOrDefault(g => g.Id == user.GroupID)
            };

            UserDao? result = _repositoryUserImpl.UpdateUser(userLocalEnity);

            if (result == null)
            {
                throw new Exception("Ошибка при обновлении пользователя.");
            }

            var groupEntity = _repositoryGroupImpl.GetAllGroup().FirstOrDefault(g => g.Id == result.GroupID);

            if (groupEntity == null)
            {
                throw new Exception("Группа не найдена.");
            }

            return new UserDao
            {
                FIO = result.FIO,
                Guid = result.Guid,
                Group = new GroupDao
                {
                    Id = groupEntity.Id,
                    Name = groupEntity.Name
                }
            };
        }


        public UserDao FindUserByGuid(Guid userGuid)
        {
            var user = _repositoryUserImpl.GetAllUsers
                .FirstOrDefault(u => u.Guid == userGuid);

            if (user == null)
            {
                throw new Exception("Пользователь не найден.");
            }

            var group = _repositoryGroupImpl.GetAllGroup()
                .FirstOrDefault(g => g.Id == user.GroupID);

            if (group == null)
            {
                throw new Exception("Группа не найдена.");
            }

            return new UserDao
            {
                FIO = user.FIO,
                Guid = user.Guid,
                Group = new GroupDao { Id = group.Id, Name = group.Name }
            };
        }
        public void AddUser(string fio, int GroupID)
        {
            try
            {
                UserDao newUser = new UserDao
                {
                    FIO = fio,
                    Guid = Guid.NewGuid(),
                    GroupID = GroupID
                };
                _repositoryUserImpl.AddUser(newUser);
                Console.WriteLine($"\nПользователь {fio} добавлен в группу с ID {GroupID}.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }
    }

}
