using System;
using System.Collections.Generic;

namespace Redakt.Model
{
    public class PageType : PersistedEntity
    {
        #region [ Constructors ]
        public PageType()
        {
            this.CompositedPageTypeIds = new List<string>();
            this.Fields = new List<FieldDefinition>();
        }
        #endregion

        #region [ Model Properties ]
        public string Name { get; set; }

        public List<string> CompositedPageTypeIds { get; set; }

        public List<FieldDefinition> Fields { get; set; }

        public string IconClass { get; set; }
        #endregion
    }
}
