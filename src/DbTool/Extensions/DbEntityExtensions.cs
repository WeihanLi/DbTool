using DbTool.Core.Entity;

// ReSharper disable once CheckNamespace
namespace DbTool
{
    public static class DbEntityExtensions
    {
        public static string GetFullTableName(this TableEntity tableEntity)
        {
            var fullName = string.IsNullOrEmpty(tableEntity.TableSchema)
                ? tableEntity.TableName
                : $"{tableEntity.TableSchema}.{tableEntity.TableName}";
            return fullName;
        }
    }
}
