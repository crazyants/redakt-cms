using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public class FieldDefinition
    {
        public string Key { get; set; }

        public string Label { get; set; }

        public string FieldTypeId { get; set; }

        public string GroupName { get; set; }

        public string SectionName { get; set; }
    }
}
