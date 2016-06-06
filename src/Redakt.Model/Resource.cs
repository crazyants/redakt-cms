using System;
using System.Collections.Generic;

namespace Redakt.Model
{
    public class Resource : PersistedEntity
    {
        #region [ Model Properties ]
        public string Name { get; set; }

        public string ContentId { get; set; }

        public string ContentTypeId { get; set; }
        #endregion
    }
}
