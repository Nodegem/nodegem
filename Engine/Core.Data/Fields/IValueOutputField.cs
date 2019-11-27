using System.Threading.Tasks;

namespace Nodegem.Engine.Data.Fields
{
    public interface IValueOutputField : IValueField
    {
        Task<object> GetValueAsync(IFlow flow);
    }
}