using MongoDB.Bson;
using System;

namespace Redakt.Model
{
    public abstract class PersistedEntity : IEntity
    {
        //private string _id;

        protected PersistedEntity()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            DbUpdated = DbCreated = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public DateTime DbCreated { get; }

        public DateTime DbUpdated { get; set; }
    }
}
