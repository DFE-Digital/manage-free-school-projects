using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Models
{
	public class StartEndModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			Type modelType = ValidateBindingContext(bindingContext);

			var startYearModelName = $"{bindingContext.ModelName}-startyear";
			var endYearModelName = $"{bindingContext.ModelName}-endyear";

			var startValueProviderResult = bindingContext.ValueProvider.GetValue(startYearModelName);
			var endValueProviderResult = bindingContext.ValueProvider.GetValue(endYearModelName);

			if (IsOptionalOrFieldTypeMismatch(bindingContext, startValueProviderResult, endValueProviderResult))
			{
				if (modelType == typeof(string))
				{
					if (IsEmpty(startValueProviderResult, endValueProviderResult))
					{
						bindingContext.Result = ModelBindingResult.Success("");
					}
					else
					{
						bindingContext.Result = ModelBindingResult.Success(null);
					}
				}
				else
				{
					bindingContext.Result = ModelBindingResult.Failed();
				}

				return Task.CompletedTask;
			}

			var displayName = bindingContext.ModelMetadata.DisplayName;

			IDateValidationMessageProvider page =
				(bindingContext.ActionContext as Microsoft.AspNetCore.Mvc.RazorPages.PageContext)?.ViewData.Model as IDateValidationMessageProvider;

			string startYear = startValueProviderResult.FirstValue;
			string endYear = endValueProviderResult.FirstValue;

            bool startParsed = int.TryParse(startYear, out int start);
            bool EndParsed = int.TryParse(endYear, out int end);

			if (!startParsed)
			{
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Start year must be numbers, like 2024");
                bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
                bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (!ValidateYearFormat(startYear))
            {
				bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Start year must begin with 20");
				bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
				bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

			if (startYear.Length != 4) {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Start year must be 4 numbers");
                bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
                bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (startYear.ToInt() < 2000 || startYear.ToInt() > 2050)
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName, "Start year must be between 2000 and 2050");
                return Task.CompletedTask;
            }

            if (!EndParsed)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "End year must be numbers, like 2026");
                bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
                bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (!ValidateYearFormat(endYear))
			{
				bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
				bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
				bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "End year must begin with 20");
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

            if (endYear.Length != 4)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "End year must be 4 numbers");
                bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
                bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (endYear.ToInt() < 2000 || endYear.ToInt() > 2050)
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName, "End year must be between 2000 and 2050");
                return Task.CompletedTask;
            }

            if (start >= end)
			{
				bindingContext.ModelState.SetModelValue(startYearModelName, startValueProviderResult);
				bindingContext.ModelState.SetModelValue(endYearModelName, endValueProviderResult);
				bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "End year must be after the start year");
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

			bindingContext.Result = ModelBindingResult.Success($"20{startYear.Substring(2, 2)}/{endYear.Substring(2, 2)}");

			return Task.CompletedTask;
		}

		private static bool ValidateYearFormat(string year)
		{
			return Regex.Match(year, "^20", RegexOptions.None, TimeSpan.FromSeconds(5)).Success;
		}

        private static Type ValidateBindingContext(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var modelType = bindingContext.ModelType;
			if (modelType != typeof(string))
			{
				throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
			}

			return modelType;
		}

		private static bool IsEmpty(ValueProviderResult startValueProviderResult, ValueProviderResult endValueProviderResult)
		{
			return startValueProviderResult.FirstValue == string.Empty
				   && endValueProviderResult.FirstValue == string.Empty;
		}

		private static bool IsOptionalOrFieldTypeMismatch(ModelBindingContext bindingContext, ValueProviderResult startValueProviderResult, ValueProviderResult endValueProviderResult)
		{
			return string.IsNullOrWhiteSpace(startValueProviderResult.FirstValue)
				   && string.IsNullOrWhiteSpace(endValueProviderResult.FirstValue)
				   && !bindingContext.ModelMetadata.IsRequired
				   || startValueProviderResult == ValueProviderResult.None
				   && endValueProviderResult == ValueProviderResult.None;
		}
	}
}
