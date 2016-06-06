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

        public IReadOnlyCollection<string> AncestorIds { get; private set; }

        public bool HasChildren { get; set; }

        public string PageTypeId { get; set; }

        public string TemplateId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime? PublishedAt { get; set; }

        public string PublishedByUserId { get; set; }

        public bool IsPublished => PublishedAt <= DateTime.UtcNow;
        #endregion

        #region [ Methods ]
        public bool IsNew()
        {
            return this.Id == null;
        }

        public void SetParent(Page parent)
        {
            if (parent == null)
            {
                this.AncestorIds = new List<string>();
            }
            else
            {
                var ancestors = parent.AncestorIds.ToList();
                ancestors.Add(parent.Id);
                this.AncestorIds = ancestors;
            }
        }
        #endregion
    }
}
