using Redakt.Model;
using System.Collections.Generic;
using System.Linq;

namespace Redakt.BackOffice.Api.Models
{
    public class FieldDefinitionModel
    {
        public FieldDefinitionModel(FieldDefinition fieldDefinition, IEnumerable<FieldType> fieldTypes, IEnumerable<IFieldEditor> fieldEditors)
        {
            this.Key = fieldDefinition.Key;
            this.Label = fieldDefinition.Label;
            this.FieldTypeId = fieldDefinition.FieldTypeId;
            this.GroupName = fieldDefinition.GroupName;
            this.SectionName = fieldDefinition.SectionName;

            var fieldType = fieldTypes.FirstOrDefault(x => x.Id == fieldDefinition.FieldTypeId);
            if (fieldType != null)
            {
                this.EditorSettings = fieldType.FieldEditorSettings;
                var editor = fieldEditors.FirstOrDefault(x => x.Id == fieldType.FieldEditorId);
                if (editor != null) this.EditorElementName = editor.UiElementName;
            }
        }

        public string Key { get; set; }
        public string Label { get; set; }
        public string FieldTypeId { get; set; }
        public string GroupName { get; set; }
        public string SectionName { get; set; }
        public string EditorElementName { get; set; }
        public object EditorSettings { get; set; }
    }
}
