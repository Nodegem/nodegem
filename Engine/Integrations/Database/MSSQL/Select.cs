using System.Data.Common;
using System.Data.SqlClient;
using Nodegem.Engine.Data.Attributes;
using SqlKata.Compilers;

namespace Nodegem.Engine.Integrations.Database.MSSQL
{
    [DefinedNode("7F3B7C35-9DCB-4DE6-A531-3A67D80B3406")]
    [NodeNamespace("Third Party.MSSQL")]
    public class Select : DatabaseCompilerNode
    {
        protected override DbConnection CreateConnection(string connectionString)
            => new SqlConnection(connectionString);
        
        protected override Compiler BuildCompiler()
            => new SqlServerCompiler();
    }
}