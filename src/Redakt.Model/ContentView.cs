using Redakt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public class ContentView
    {
        public PageContent Content { get; set; }
        public PageType PageType { get; set; }

        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            var fieldDefinition = PageType.Fields.FirstOrDefault(ft => ft.Key == key);
            if (fieldDefinition == null) return defaultValue;

            object fieldValue;
            if (!Content.Fields.TryGetValue(key, out fieldValue)) return defaultValue;

            return defaultValue;//fieldDefinition.RequestValue(fieldValue, defaultValue);
        }
    }
}
