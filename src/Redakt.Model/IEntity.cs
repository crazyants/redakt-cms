using System;

namespace Redakt.Model
{
    public interface IEntity
    {
        string Id { get; set; }
        DateTime DbCreated { get; }
        DateTime DbUpdated { get; set; }
    }
}
