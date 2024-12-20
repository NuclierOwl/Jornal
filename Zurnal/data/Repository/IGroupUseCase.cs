using data.RemoteData.RemoteDataBase.DAO;
using domain.Request;

namespace Zurnal.data.Repository
{
    public interface IGroupUseCase
    {
        public IEnumerable<GroupDao> GetGroupsWithStudents();
        public void AddGroup(AddGroupRequest addGroupRequest);
        public void AddGroupWithStudents(AddGroupWithStudentsRequest addGroupWithStudents);
        public void RemoveAllStudentsFromGroup(int ID);
        public void AddStudentToGroup(int groupId, UserDao student);
    }
}
