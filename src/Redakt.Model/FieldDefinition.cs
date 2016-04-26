using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public class FieldDefinition: PersistedEntity
    {
        public string FieldTypeId { get; set; }

        public string TabName { get; set; }
    }
}
