namespace data.Exception
{
    public class RepositoryException : IOException
    {
        public RepositoryException(string message) : base(message) { }
    }
}