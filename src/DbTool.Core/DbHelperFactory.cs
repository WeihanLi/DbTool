namespace DbTool.Core
{
    public interface IDbHelperFactory
    {
        IDbHelper GetDbHelper(IDbProvider dbProvider, string connectionString);
    }

    public sealed class DbHelperFactory : IDbHelperFactory
    {
        public IDbHelper GetDbHelper(IDbProvider dbProvider, string connectionString)
        {
            return new DbHelper(dbProvider, connectionString);
        }
    }
}
