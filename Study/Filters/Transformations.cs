using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Study.Filters
{
    public static class Transformations
    {
        public static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, string> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                string value = item.Value; // Значение, которое нужно преобразовать
                TimeSpan timeSpan;

                if (TimeSpan.TryParse(value, out timeSpan))
                {
                    try
                    {
                        object newA = TimeSpan.ParseExact(item.Value, "h\\:mm\\:ss", CultureInfo.InvariantCulture);
                        if (newA != null)
                        {
                            t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                        }
                    }
                    catch
                    {

                    }
                }
                if (item.Value != null)
                {
                    try
                    {
                        object newA = Convert.ChangeType(item.Value, newT);
                        t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                    }
                    catch
                    {

                    }
                }
                else if (int.TryParse(value, out int intValue))
                {
                    object newA = int.Parse(item.Value);


                    t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                }

            }
            return t;
        }

    }
}
