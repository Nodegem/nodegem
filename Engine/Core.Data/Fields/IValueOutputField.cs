namespace Nodester.Engine.Data.Fields
{
    public interface IValueOutputField : IValueField
    {
        object GetValue(IFlow flow);
    }
}