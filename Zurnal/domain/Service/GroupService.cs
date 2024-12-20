using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;
using domain.Request;
using domain.UseCase;

namespace domain.Service
{
    public class GroupService : GroupUseCase
    {
        public readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository): base(groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public void AddGroup(AddGroupRequest addGroupRequest)
        {
            _groupRepository.AddGroup(new GroupDao { Name = addGroupRequest.Name });
        }
        public IEnumerable<GroupDao> GetGroupsWithStudents()
        {
            return _groupRepository.GetAllGroup().Select(
                group => new GroupDao
                {
                    Id = group.Id,
                    Name = group.Name,
                    Users = group.Users.Select(
                        user => new UserDao
                        {
                            Guid = user.Guid,
                            FIO = user.FIO,
                            Group = new GroupDao
                            {
                                Id = group.Id,
                                Name = group.Name,
                            }
                        }).ToList()
                }).ToList();
        }
    }
}
