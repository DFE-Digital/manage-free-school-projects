﻿namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
	public class TextAreaInputViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public string Label { get; set; }
		public string ErrorMessage { get; set; }
		public int Rows { get; set; }
		
		public bool BoldLabel { get; set; }
		public string Hint { get; set; }
		public bool RichText { get; set; }
		public bool HeadingLabel { get; set; }
		public string TestId { get; set; }
	}
}
