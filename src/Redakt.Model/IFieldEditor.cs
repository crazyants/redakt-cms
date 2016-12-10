using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public interface IFieldEditor
    {
        string Id { get; }

        string Name { get; }

        string UiElementName { get; }
    }

    public interface IFieldEditor<T>: IFieldEditor
    {
        bool HasValue { get; }
        T GetValue(T defaultValue = default(T));
        TValue GetValue<TValue>(TValue defaultValue = default(TValue));
        void SetValue(object val);
        //object Configuration { get; set; }
    }

    public interface IConfigurableFieldEditor<T, TConfig>: IFieldEditor<T>
    {
        TConfig Configuration { get; set; }
    }
}
