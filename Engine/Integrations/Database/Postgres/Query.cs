using System.Data.Common;
using Nodegem.Engine.Data.Attributes;
using Npgsql;

namespace Nodegem.Engine.Integrations.Database.Postgres
{
    [DefinedNode("C3F73AD9-EEEB-4F29-AC84-711F3339DFF6")]
    [NodeNamespace("Integrations.Postgres")]
    public class Query : DatabaseQueryNode
    {
        protected override DbConnection CreateConnection(string connectionString)
            => new NpgsqlConnection(connectionString);
    }
}