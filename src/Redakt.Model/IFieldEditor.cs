using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redakt.Model
{
    public interface IFieldEditor<T>
    {
        string Name { get; }
        bool HasValue { get; }
        T GetValue(T defaultValue = default(T));
        TValue GetValue<TValue>(TValue defaultValue = default(TValue));
        void SetValue(object val);
        object Configuration { get; set; }
    }
}
