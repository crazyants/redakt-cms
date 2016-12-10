using Redakt.Model;
using System.Collections.Generic;
using System.Linq;

namespace Redakt.BackOffice.Api.Models
{
    public class PageTypeListItemModel
    {
        public PageTypeListItemModel(PageType pageType)
        {
            this.Id = pageType.Id;
            this.Name = pageType.Name;
            this.IconClass = pageType.IconClass;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
    }

    public class PageTypeModel
    {
        public PageTypeModel(PageType pageType, IEnumerable<FieldType> fieldTypes, IEnumerable<IFieldEditor> fieldEditors)
        {
            this.Id = pageType.Id;
            this.Name = pageType.Name;
            this.IconClass = pageType.IconClass;
            this.Fields = pageType.Fields.Select(fieldDefinition => new FieldDefinitionModel(fieldDefinition, fieldTypes, fieldEditors)).ToList();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IconClass { get; set; }
        public List<FieldDefinitionModel> Fields { get; set; }
    }
}
