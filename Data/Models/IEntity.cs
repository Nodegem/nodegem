using System;

namespace Nodegem.Data.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime LastUpdated { get; set; }
    }
}