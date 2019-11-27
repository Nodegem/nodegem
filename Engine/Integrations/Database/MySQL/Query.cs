using System.Data.Common;
using MySql.Data.MySqlClient;
using Nodegem.Engine.Data.Attributes;

namespace Nodegem.Engine.Integrations.Database.MySQL
{
    [DefinedNode("7D8D2735-428A-4F22-B0EB-F424E35FEA2F")]
    [NodeNamespace("Third Party.MySQL")]
    public class Query : DatabaseQueryNode
    {
        protected override DbConnection CreateConnection(string connectionString)
            => new MySqlConnection(connectionString);
    }
}