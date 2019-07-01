namespace Nodester.Data.Models
{
    public class ActiveEntity : BaseEntity, IActiveEntity
    {
        public bool IsActive { get; set; } = true;
    }
}