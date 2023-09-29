using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIVisualwfpnew.Helpers
{
    public static class Extensions
    {
        public static string GetDescription(this System.Enum value)
        {
            var description = value.ToString();
            var attribute = GetAttribute<DescriptionAttribute>(value);
            if (attribute != null)
                description = attribute.Description;

            return description;
        }

        public static T GetAttribute<T>(System.Enum value) where T : System.Attribute
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = ((T[])field.GetCustomAttributes(typeof(T), false)).FirstOrDefault();
            return attribute;
        }
    }
}
