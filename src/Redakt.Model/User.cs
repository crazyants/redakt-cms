using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public class User: PersistedEntity
    {
        public string Name { get; set; }
    }
}
