

namespace ERC_test_task.Sqlite3.DbTables.Interface
{
    internal interface IDbTable
    {
        
        internal string TableName { get; }
        internal string[] FieldsCreateText { get; }
    }
}
