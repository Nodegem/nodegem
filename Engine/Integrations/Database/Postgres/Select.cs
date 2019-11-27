using System.Data.Common;
using Nodegem.Engine.Data.Attributes;
using Npgsql;
using SqlKata.Compilers;

namespace Nodegem.Engine.Integrations.Database.Postgres
{
    [DefinedNode("27A97B70-A0B0-485F-B186-71CF4AADD20D")]
    [NodeNamespace("Third Party.Postgres")]
    public class Select : DatabaseCompilerNode
    {
        protected override DbConnection CreateConnection(string connectionString)
            => new NpgsqlConnection(connectionString);

        protected override Compiler BuildCompiler()
            => new PostgresCompiler();
    }
}