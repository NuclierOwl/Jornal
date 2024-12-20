namespace data.Exception
{
    public class GroupNotFoundException : RepositoryException
    {
        public GroupNotFoundException(int GroupID)
            : base($"Группа с ID {GroupID} не найдена.") { }
    }
}