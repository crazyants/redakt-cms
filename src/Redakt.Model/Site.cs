using System;
using System.Collections.Generic;

namespace Redakt.Model
{
    public class Site : PersistedEntity
    {
        #region [ Model Properties ]
        public string HomePageId { get; set; }

        public string Name { get; set; }
        #endregion
    }
}
