using System;
using System.Collections.Generic;

namespace Redakt.Model
{
    public class PageContent : PersistedEntity
    {
        public PageContent()
        {
            this.Fields = new Dictionary<string, object>();
        }

        #region [ Model Properties ]
        public string PageId { get; set; }

        public string Culture { get; set; }

        public DateTime Created { get; set; }

        public Dictionary<string, object> Fields { get; set; } 

        public string CreatedUserId { get; set; }

        #endregion
    }
}
