using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace PrimesWithCircles.Extensions
{
    [MarkupExtensionReturnType(typeof(IEnumerable))]
    public class EnumDescription : MarkupExtension
    {
        public Type EnumType { get; set; }

        public EnumDescription()
        {
        }

        public EnumDescription(Type enumType)
        {
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == null || !EnumType.IsEnum)
                return null;

            return Enum.GetValues(EnumType)
                .Cast<Enum>()
                .Select(e => new
                {
                    Value = e,
                    Description = GetEnumDescription(e)
                })
                .ToList();
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                    return attributes[0].Description;
            }
            return value.ToString();
        }
    }
}