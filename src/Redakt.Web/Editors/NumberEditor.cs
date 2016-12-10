﻿using System;
using System.Globalization;
using Redakt.Model;

namespace Redakt.Web.Editors
{
    public class NumberEditor: IFieldEditor<decimal?>, IFieldEditor
    {
        private decimal? _value;

        public string Id => "479a9f0acddd4af3bb3ff98d492f0e1a";

        public string Name => "Number Editor";

        public string UiElementName => "number-editor";

        public bool HasValue => _value.HasValue;

        public decimal? GetValue(decimal? defaultValue = null)
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
    }
}
