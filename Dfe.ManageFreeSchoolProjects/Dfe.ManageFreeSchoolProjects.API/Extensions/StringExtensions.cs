using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string value)
        {
            var succeeded = int.TryParse(value, out var result);

            return succeeded ? result : 0;
        }

        public static decimal ToDecimal(this string value)
        {
            var succeeded = decimal.TryParse(value, out var result);

            return succeeded ? result : 0;
        }

        public static T ToEnumFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (string.Compare(attribute.Description, description, true) == 0)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Unable to parse enum from description: ", nameof(description));
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
