using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PayPal.Util
{
    public class ReflectionEnumUtil
    {
        public static string getDescription(Enum value)
        {
            string description = string.Empty;
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        public static object getValue(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (getDescription((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }
            return null;
        }
    }
}
