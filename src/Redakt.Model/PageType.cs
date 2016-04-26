using System;
using System.Collections.Generic;

namespace Redakt.Model
{
    public class PageType : PersistedEntity
    {
        #region [ Model Properties ]
        public string Name { get; set; }

        public List<string> CompositedContentTypeIds { get; set; }

        public Dictionary<string, FieldDefinition> Fields { get; set; }
        #endregion
    }
}
