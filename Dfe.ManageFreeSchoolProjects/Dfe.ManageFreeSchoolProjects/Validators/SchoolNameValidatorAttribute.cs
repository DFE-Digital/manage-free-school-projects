﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Text;

namespace Dfe.ManageFreeSchoolProjects.Models;

public class SchoolNameValidatorAttribute : ValidationAttribute
{
    private const string AllowSpecialCharactersPattern = @"^(?=.*[a-zA-Z])[a-zA-Z0-9'(),\s]*$";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var valueAsString = (string) value;
        
        if (!valueAsString.ToList().Exists(x => x.IsLetter()))
        {
            return new ValidationResult("Please enter some letters.");
        }
        
        var specialCharactersRegex = new Regex(AllowSpecialCharactersPattern, RegexOptions.None, TimeSpan.FromSeconds(30));
        var match = specialCharactersRegex.Match(valueAsString);
        
        return match.Success
            ? ValidationResult.Success
            : new ValidationResult("Please use valid characters. Valid characters are: A-Z, apostrophes, parentheses and commas.");
    }
}