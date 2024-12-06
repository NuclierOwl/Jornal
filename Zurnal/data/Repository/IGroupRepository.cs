using data.RemoteData.RemoteDataBase.DAO;
using domain.Models;

namespace data.Repository
{
    public interface IGroupRepository
    {
        List<GroupDao> GetAllGroup();
        bool RemoveGroupById(int groupID);
        bool UpdateGroupById(int groupID, GroupDao updatedGroup);
        GroupDao GetGroupById(int groupID);
        bool AddGroup(GroupDao newGroup);

    }
}