namespace Nodester.Engine.Data.Definitions
{
    public class ValueInputDefinition : ValueFieldDefinition
    {
        public object DefaultValue { get; set; }
        public bool IsEditable { get; set; }
    }
}