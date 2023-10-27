﻿using System.ComponentModel;
using System.Reflection;

namespace Dfe.ManageFreeSchoolProjects.Extensions
{
	public static class EnumExtensions
	{
		public static string ToDescription<T>(this T source)
		{
			if (source == null) return string.Empty;

			FieldInfo fi = source.GetType().GetField(source.ToString());

			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute), false);

			return attributes.Length > 0 
				? attributes[0].Description 
				: source.ToString();
		}
	}
}
