using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;

public class GroupRepositoryImpl : IGroupRepository
{
    private List<GroupDao> _groups = new List<GroupDao>();

    public GroupDao? GetGroupById(int groupId)
    {
        return _groups.FirstOrDefault(g => g.Id == groupId);
    }

    // Метод для получения всех групп
    public List<GroupDao> GetAllGroup() => _groups;

    // Метод для добавления новой группы
    public bool AddGroup(GroupDao group)
    {
        if (_groups.Any(g => g.Id == group.Id))
            return false; 

        group.Id = _groups.Any() ? _groups.Max(g => g.Id) + 1 : 1;
        _groups.Add(group);
        return true; 
    }

    // Метод для обновления существующей группы
    public bool UpdateGroupById(int groupID, GroupDao updatedGroup)
    {
        var existingGroup = GetGroupById(groupID);
        if (existingGroup == null)
            return false; 

        existingGroup.Name = updatedGroup.Name;
        return true; 
    }

    
    public bool RemoveGroupById(int groupID)
    {
        var existingGroup = GetGroupById(groupID);
        if (existingGroup == null)
            return false; 

        _groups.Remove(existingGroup);
        return true; 
    }

    public IEnumerable<GroupDao> GetGroupsWithStudents()
    {
        throw new NotImplementedException();
    }
}
