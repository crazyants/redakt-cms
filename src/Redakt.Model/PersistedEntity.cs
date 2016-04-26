using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Redakt.Model
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class PersistedEntity : IEntity
    {
        private string _id;

        protected PersistedEntity()
        {
            _id = ObjectId.GenerateNewId().ToString();
            DbUpdated = DbCreated = DateTime.UtcNow;
        }

        [BsonId]
        public string Id
        {
            get { return _id; }
            set { if (!string.IsNullOrEmpty(value)) _id = value; }
        }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DbCreated { get; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DbUpdated { get; set; }
    }
}
