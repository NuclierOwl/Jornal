using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;
using remoteData.RemoteDataBase;

public class SQLGroupRepositoryImpl : IGroupRepository
{
    private readonly RemoteDatabaseContext _remoteDatabaseContext;

    public SQLGroupRepositoryImpl(RemoteDatabaseContext remoteDatabaseContext)
    {
        _remoteDatabaseContext = remoteDatabaseContext;
    }

    
    public GroupDao? GetGroupById(int groupId)
    {
        var groupDao = _remoteDatabaseContext.groups.FirstOrDefault(g => g.Id == groupId);
        return groupDao != null ? new GroupDao { Id = groupDao.Id, Name = groupDao.Name } : null;
    }

    
    public List<GroupDao> GetAllGroup()
    {
        return _remoteDatabaseContext.groups
            .Select(g => new GroupDao { Id = g.Id, Name = g.Name })
            .ToList();
    }

    
    public bool AddGroup(GroupDao group)
    {
        if (_remoteDatabaseContext.groups.Any(g => g.Id == group.Id))
            return false;

        var groupDao = new GroupDao { Id = group.Id, Name = group.Name };
        _remoteDatabaseContext.groups.Add(groupDao);
        _remoteDatabaseContext.SaveChanges();
        return true;
    }

    
    public bool UpdateGroupById(int groupID, GroupDao updatedGroup)
    {
        var existingGroup = _remoteDatabaseContext.groups.FirstOrDefault(g => g.Id == groupID);
        if (existingGroup == null)
            return false;

        existingGroup.Name = updatedGroup.Name;
        _remoteDatabaseContext.SaveChanges();
        return true;
    }

    
    public bool RemoveGroupById(int groupID)
    {
        var existingGroup = _remoteDatabaseContext.groups.FirstOrDefault(g => g.Id == groupID);
        if (existingGroup == null)
            return false;

        _remoteDatabaseContext.groups.Remove(existingGroup);
        _remoteDatabaseContext.SaveChanges();
        return true;
    }
}
