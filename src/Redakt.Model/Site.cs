using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Redakt.Model
{
    public class Site : PersistedEntity
    {
        #region [ Model Properties ]
        public string Name { get; set; }
        #endregion
    }
}
