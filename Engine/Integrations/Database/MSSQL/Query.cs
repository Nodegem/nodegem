using System.Data.Common;
using System.Data.SqlClient;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Integrations.Database.MSSQL
{
    [DefinedNode("5575528D-1122-472D-8889-736B51EFE3E0")]
    [NodeNamespace("Integrations.MSSQL")]
    public class Query : DatabaseQueryNode
    {
        protected override DbConnection CreateConnection(string connectionString)
             => new SqlConnection(connectionString);
    }
}