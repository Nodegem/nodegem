using System.Threading.Tasks;

namespace Nodester.Engine.Data.Fields
{
    public interface IValueOutputField : IValueField
    {
        Task<object> GetValueAsync(IFlow flow);
    }
}