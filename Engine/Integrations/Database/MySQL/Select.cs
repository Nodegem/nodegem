using System.Data.Common;
using MySql.Data.MySqlClient;
using Nodegem.Engine.Data.Attributes;
using SqlKata.Compilers;

namespace Nodegem.Engine.Integrations.Database.MySQL
{
    [DefinedNode("CE074ADF-4CD7-4245-B377-AB9EEF0DA786")]
    [NodeNamespace("Integrations.MySQL")]
    public class Select : DatabaseCompilerNode
    {
        protected override DbConnection CreateConnection(string connectionString)
            => new MySqlConnection(connectionString);

        protected override Compiler BuildCompiler()
            => new MySqlCompiler();
    }
}