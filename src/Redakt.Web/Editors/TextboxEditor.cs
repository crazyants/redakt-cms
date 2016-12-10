using System;
using System.Globalization;
using Redakt.Model;

namespace Redakt.Web.Editors
{
    public class TextboxEditor: IFieldEditor<string>
    {
        private string _value;

        public string Id => "15317e3a67044ee5af7ccba1e9537cde";

        public string Name => "Textbox Editor";

        public string UiElementName => "textbox-editor";

        public bool HasValue => _value != null;

        public string GetValue(string defaultValue = null)
        {
            return _value ?? defaultValue;
        }

        public TValue GetValue<TValue>(TValue defaultValue = default(TValue))
        {
            if (_value == null) return defaultValue;

            //if (typeof(TValue) == typeof(decimal)) return (TValue)(object)_value.Value;
            //if (typeof(TValue) == typeof(int)) return (TValue)(object)Convert.ToInt32(_value.Value);
            //if (typeof(TValue) == typeof(double)) return (TValue)(object)Convert.ToDouble(_value.Value);
            //if (typeof(TValue) == typeof(float)) return (TValue)(object)Convert.ToSingle(_value.Value);
            //if (typeof(TValue) == typeof(short)) return (TValue)(object)Convert.ToInt16(_value.Value);
            //if (typeof(TValue) == typeof(long)) return (TValue)(object)Convert.ToInt64(_value.Value);
            //if (typeof(TValue) == typeof(byte)) return (TValue)(object)Convert.ToByte(_value.Value);
            //if (typeof(TValue) == typeof(string)) return (TValue)(object)Convert.ToString(_value.Value, CultureInfo.CurrentCulture);

            throw new NotSupportedException("Cannot convert to this type.");
        }

        public void SetValue(object val)
        {
            try
            {
                _value = Convert.ToString(val);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
