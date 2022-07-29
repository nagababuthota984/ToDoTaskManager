using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Helpers
{
    public class EnumHelper
    {
        public static string GetDescription<T>(T value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());

            DescriptionAttribute? attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

    }
}
