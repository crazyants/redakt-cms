using System;

namespace Redakt.Core.Cache
{
    internal class CacheItem
    {
        public CacheItem(object value)
        {
            this.Inserted = this.LastHit = DateTime.UtcNow;
            this.Value = value;
        }

        internal DateTime Inserted { get; set; }
        internal DateTime LastHit { get; set; }
        internal object Value { get; set; }
    }
}
