﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Text;
using Dfe.ManageFreeSchoolProjects.Constants;

namespace Dfe.ManageFreeSchoolProjects.Models;

public class SchoolNameValidatorAttribute : ValidationAttribute
{
    private const string AllowSpecialCharactersPattern = @"^(?=.*[a-zA-Z])[a-zA-Z0-9'(),\s]*$";
    private const int MaxLength = 100;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var valueAsString = (string) value;

        if(valueAsString.Length > MaxLength)
            return new ValidationResult(string.Format(ValidationConstants.TextValidationMessage, "school name", MaxLength));

        var specialCharactersRegex = new Regex(AllowSpecialCharactersPattern, RegexOptions.None, TimeSpan.FromSeconds(30));
        var match = specialCharactersRegex.Match(valueAsString);
        
        return match.Success
            ? ValidationResult.Success
            : new ValidationResult("School name must not include special characters other than , ( ) '");
    }
}