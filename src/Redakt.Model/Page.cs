using System;
using System.Collections.Generic;
using System.Linq;

namespace Redakt.Model
{
    public class Page : PersistedEntity
    {
        public Page()
        {
            this.AncestorIds = new List<string>();
        }

        #region [ Model Properties ]
        public string Name { get; set; }

        public string ParentId => this.AncestorIds.LastOrDefault();

        public IEnumerable<string> AncestorIds { get; set; }

        public string PageTypeId { get; set; }

        public string TemplateId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime? PublishedAt { get; set; }

        public string PublishedByUserId { get; set; }

        public bool IsPublished => PublishedAt <= DateTime.UtcNow;
        #endregion
    }
}
