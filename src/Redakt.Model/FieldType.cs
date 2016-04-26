using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public class FieldType : PersistedEntity
    {
        public string Name { get; set; }

        public string FieldEditorId { get; set; }

        public object FieldEditorSettings { get; set; }
    }
}
