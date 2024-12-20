using data.RemoteData.RemoteDataBase.DAO;

namespace data.Repository
{
    public interface IGroupRepository
    {
        List<GroupDao> GetAllGroup();
        bool RemoveGroupById(int GroupID);
        bool UpdateGroupById(int GroupID, GroupDao updatedGroup);
        GroupDao GetGroupById(int GroupID);
        bool AddGroup(GroupDao newGroup);
        public IEnumerable<GroupDao> GetGroupsWithStudents();

    }
}