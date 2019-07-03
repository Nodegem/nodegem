namespace Nodester.Engine.Data.Fields
{
    public interface IMacroValueInputField : IValueOutputField
    {
        object DefaultValue { get; }
    }
}