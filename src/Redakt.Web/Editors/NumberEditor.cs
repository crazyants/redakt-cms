using System;
using System.Globalization;
using Redakt.Model;

namespace Redakt.Web.Editors
{
    public class NumberEditorConfiguration
    {
        public bool IsDecimal { get; set; }
        public int? DecimalPlaces { get; set; }
    }

    public class NumberEditor: IConfigurableFieldEditor<decimal, NumberEditorConfiguration>
    {
        private NumberEditorConfiguration _config;
        private decimal? _value;

        public string Name => "Number Editor";

        public bool HasValue => _value.HasValue;

        public decimal GetValue(decimal defaultValue = 0)
        {
            return _value ?? defaultValue;
        }

        public TValue GetValue<TValue>(TValue defaultValue = default(TValue))
        {
            if (!_value.HasValue) return defaultValue;

            if (typeof(TValue) == typeof(decimal)) return (TValue)(object)_value.Value;
            if (typeof(TValue) == typeof(int)) return (TValue)(object)Convert.ToInt32(_value.Value);
            if (typeof(TValue) == typeof(double)) return (TValue)(object)Convert.ToDouble(_value.Value);
            if (typeof(TValue) == typeof(float)) return (TValue)(object)Convert.ToSingle(_value.Value);
            if (typeof(TValue) == typeof(short)) return (TValue)(object)Convert.ToInt16(_value.Value);
            if (typeof(TValue) == typeof(long)) return (TValue)(object)Convert.ToInt64(_value.Value);
            if (typeof(TValue) == typeof(byte)) return (TValue)(object)Convert.ToByte(_value.Value);
            if (typeof(TValue) == typeof(string)) return (TValue)(object)Convert.ToString(_value.Value, CultureInfo.CurrentCulture);

            throw new NotSupportedException("Cannot convert to this type.");
        }

        public void SetValue(object val)
        {
            try
            {
                _value = Convert.ToDecimal(val);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public NumberEditorConfiguration Configuration { get; set; }
    }
}
