using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;
using remoteData.RemoteDataBase;

public class SQLGroupRepositoryImpl : IGroupRepository
{
    public readonly RemoteDatabaseContext _remoteDatabaseContext;

    public SQLGroupRepositoryImpl(RemoteDatabaseContext remoteDatabaseContext)
    {
        _remoteDatabaseContext = remoteDatabaseContext;
    }

    
    public GroupDao? GetGroupById(int GroupID)
    {
        var GroupDao = _remoteDatabaseContext.groups.FirstOrDefault(g => g.Id == GroupID);
        return GroupDao != null ? new GroupDao { Id = GroupDao.Id, Name = GroupDao.Name } : null;
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

        var GroupDao = new GroupDao { Id = group.Id, Name = group.Name };
        _remoteDatabaseContext.groups.Add(GroupDao);
        _remoteDatabaseContext.SaveChanges();
        return true;
    }

    
    public bool UpdateGroupById(int GroupID, GroupDao updatedGroup)
    {
        var existingGroup = _remoteDatabaseContext.groups.FirstOrDefault(g => g.Id == GroupID);
        if (existingGroup == null)
            return false;

        existingGroup.Name = updatedGroup.Name;
        _remoteDatabaseContext.SaveChanges();
        return true;
    }

    
    public bool RemoveGroupById(int GroupID)
    {
        var existingGroup = _remoteDatabaseContext.groups.FirstOrDefault(g => g.Id == GroupID);
        if (existingGroup == null)
            return false;

        _remoteDatabaseContext.groups.Remove(existingGroup);
        _remoteDatabaseContext.SaveChanges();
        return true;
    }

    public IEnumerable<GroupDao> GetGroupsWithStudents()
    {
        throw new NotImplementedException();
    }
}
