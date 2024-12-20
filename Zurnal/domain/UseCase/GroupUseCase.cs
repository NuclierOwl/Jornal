using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;
using domain.Request;
using Zurnal.data.Repository;

namespace domain.UseCase
{
    public class GroupUseCase : IGroupUseCase
    {
        public readonly IGroupRepository _repositoryGroupImpl;
        public readonly UserRepositoryImpl _repositoryUserImpl;

        public GroupUseCase(IGroupRepository repositoryGroupImpl)
        {
            _repositoryGroupImpl = repositoryGroupImpl;
        }

        public GroupDao ValidateGroupExistence(int GroupID)
        {
            var existingGroup = _repositoryGroupImpl.GetAllGroup()
                .FirstOrDefault(g => g.Id == GroupID);

            if (existingGroup == null)
            {
                throw new ArgumentException("Группа не найдена.");
            }

            return new GroupDao
            {
                Id = existingGroup.Id,
                Name = existingGroup.Name
            };
        }

        public List<GroupDao> GetAllGroups()
        {
            return _repositoryGroupImpl.GetAllGroup()
                .Select(it => new GroupDao { Id = it.Id, Name = it.Name }).ToList();
        }

        public GroupDao FindGroupById(int GroupID)
        {
            var group = GetAllGroups().FirstOrDefault(g => g.Id == GroupID);

            if (group == null)
            {
                throw new ArgumentException("Группа не найдена.");
            }

            return group;
        }

        public void AddGroup(string Id)
        {
            var newId = _repositoryGroupImpl.GetAllGroup().Any()
                        ? _repositoryGroupImpl.GetAllGroup().Max(g => g.Id) + 1
                        : 1;

            GroupDao newGroup = new GroupDao
            {
                Id = newId,
                Name = Id
            };

            _repositoryGroupImpl.AddGroup(newGroup);
        }

        public void RemoveGroupById(int GroupID)
        {
            var existingGroup = ValidateGroupExistence(GroupID);
            List<GroupDao> _groups = GetAllGroups();

            var groupToRemove = _groups.FirstOrDefault(g => g.Id == existingGroup.Id);
            if (groupToRemove != null)
            {
                _groups.Remove(groupToRemove);
                _repositoryGroupImpl.RemoveGroupById(existingGroup.Id);
            }
            else
            {
                throw new ArgumentException("Группа не найдена.");
            }
        }

        public void AddStudentToGroup(int groupId, UserDao student)
        {
            var existingGroup = ValidateGroupExistence(groupId);
            student.GroupID = existingGroup.Id;
            _repositoryUserImpl.AddUser(student);
            Console.WriteLine($"\nПользователь {student.FIO} добавлен в группу с ID {groupId}.\n");
        }

        public bool UpdateGroup(int GroupID, string newId)
        {
            var existingGroup = _repositoryGroupImpl.GetAllGroup()
                .FirstOrDefault(g => g.Id == GroupID);

            if (existingGroup == null)
            {
                return false;
            }

            existingGroup.Name = newId;
            _repositoryGroupImpl.UpdateGroupById(existingGroup.Id, existingGroup);
            return true;
        }

        public Task<IEnumerable<GroupDao>> GetGroupsWithStudentsAsync()
        {
            return Task.FromResult(GetGroupsWithStudents());
        }

        public void AddGroup(AddGroupRequest addGroupRequest)
        {
            AddGroup(addGroupRequest.Name);
        }

        void IGroupUseCase.AddGroupWithStudents(AddGroupWithStudentsRequest addGroupWithStudents)
        {
            AddGroup(addGroupWithStudents.addGroupRequest.Name);
            int groupId = _repositoryGroupImpl.GetAllGroup().Last().Id;

            foreach (var student in addGroupWithStudents.AddStudentRequests)
            {
                try
                {
                    string fio = student.StudentName;
                    UserDao newUser = new UserDao
                    {
                        FIO = fio,
                        Guid = Guid.NewGuid(),
                        GroupID = groupId
                    };
                    _repositoryUserImpl.AddUser(newUser);
                    Console.WriteLine($"\nПользователь {fio} добавлен в группу с ID {groupId}.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}\n");
                }
            }
        }

        public IEnumerable<GroupDao> GetGroupsWithStudents()
        {
            return _repositoryGroupImpl.GetAllGroup()
                .Select(g => new GroupDao
                {
                    Id = g.Id,
                    Name = g.Name,
                    Users = g.Users
                });
        }


        public void RemoveAllStudentsFromGroup(int Id)
        {
            var group = ValidateGroupExistence(Id);
            foreach (var user in group.Users.ToList())
            {
                _repositoryUserImpl.RemoveUserByGuid(user.Guid);
            }
        }
        //public IEnumerable<GroupDao> GetGroupsWithStudents()
        //{
        //    return _repositoryGroupImpl.GetAllGroup().Select(
        //        group =>
        //        {
        //            if (group == null)
        //            {
        //                throw new ArgumentNullException(nameof(group), "Группа не может быть null.");
        //            }

        //            return new GroupDao
        //            {
        //                Id = group.Id,
        //                Name = group.Name ?? "Неизвестная группа",
        //                Users = group.Users.Select(
        //                    user => new UserDao
        //                    {
        //                        Guid = user.Guid,
        //                        FIO = user.FIO,
        //                        GroupID = user.GroupID,
        //                        Group = new GroupDao
        //                        {
        //                            Id = group.Id,
        //                            Name = group.Name ?? "Неизвестная группа",
        //                        }
        //                    }).ToList()
        //            };
        //        }).ToList();
        //}
    }
}