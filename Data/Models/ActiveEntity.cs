namespace Nodegem.Data.Models
{
    public class ActiveEntity : BaseEntity, IActiveEntity
    {
        public bool IsActive { get; set; } = true;
    }
}