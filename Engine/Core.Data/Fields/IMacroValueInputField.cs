namespace Nodester.Graph.Core.Data.Fields
{
    public interface IMacroValueInputField : IValueOutputField
    {
        object DefaultValue { get; }
    }
}