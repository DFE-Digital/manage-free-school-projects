using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Dfe.ManageFreeSchoolProjects.Extensions
{
	public static class EnumExtensions
	{
		public static string ToDescription<T>(this T source)
		{
			if (source is null) return string.Empty;

			return DescriptionOf(source) ?? source.ToString();
		}

		public static string ToDescriptionOrEmpty<T>(this T source)
		{
			if (source is null) return string.Empty;

			return DescriptionOf(source) ?? string.Empty;
		}
		private static string DescriptionOf<T>(T source)
		{
			FieldInfo fi = source.GetType().GetField(source.ToString());

			if (fi is null) return null;

			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute), false);

			return attributes.Length > 0 ? attributes[0].Description : null;
		}

		public static string ToIntString(this Enum value)
		{
			if (value == null) return string.Empty;

			return value.ToString("D");
		}

		public static T? ToEnum<T>(this string value) where T : struct
        {
			if (value == null) return null;

            return (T)Enum.Parse(typeof(T), value);
		}

		public static T FromDescription<T>(this string description) where T : struct, Enum
		{
			var match = Enum.GetValues<T>()
				.Select(value => (T?)value)
				.FirstOrDefault(value => value.Value.ToDescription() == description);

			return match ?? throw new ArgumentException($"Unknown {typeof(T).Name}: {description}");
		}
	}
}
