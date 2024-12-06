using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;
using domain.Models;



namespace domain.UseCase
{
    public class GroupUseCase
    {
        private readonly IGroupRepository _repositoryGroupImpl;

        public GroupUseCase(IGroupRepository repositoryGroupImpl)
        {
            _repositoryGroupImpl = repositoryGroupImpl;
        }


        private GroupDao ValidateGroupExistence(int groupId)
        {
            var existingGroup = _repositoryGroupImpl.GetAllGroup()
                .FirstOrDefault(g => g.Id == groupId);

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

        public List<Group> GetAllGroups()
        {
            return [.. _repositoryGroupImpl.GetAllGroup()
                .Select(it => new Group { Id = it.Id, Name = it.Name })];
        }


        public Group FindGroupById(int groupId)
        {
            var group = GetAllGroups().FirstOrDefault(g => g.Id == groupId);

            if (group == null)
            {
                throw new ArgumentException("Группа не найдена.");
            }

            return group;
        }


        public void AddGroup(string groupName)
        {
            

            var newId = _repositoryGroupImpl.GetAllGroup().Any()
                        ? _repositoryGroupImpl.GetAllGroup().Max(g => g.Id) + 1
                        : 1;

            GroupDao newGroup = new GroupDao
            {
                Id = newId,
                Name = groupName
            };

            _repositoryGroupImpl.AddGroup(newGroup);
        }

        public void RemoveGroupById(int groupId)
        {
            
            var existingGroup = ValidateGroupExistence(groupId);
            List<Group> _groups = GetAllGroups();

            // Находим группу по ID и удаляем ее
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


        // Метод для изменения названия группы
        public bool UpdateGroup(int groupId, string newGroupName)
        {
            var existingGroup = _repositoryGroupImpl.GetAllGroup()
                .FirstOrDefault(g => g.Id == groupId);

            if (existingGroup == null)
            {
                return false; // Группа с таким ID не найдена
            }

            existingGroup.Name = newGroupName;
            _repositoryGroupImpl.UpdateGroupById(existingGroup.Id, existingGroup);
            return true; // Успешное обновление
        }

    }
}