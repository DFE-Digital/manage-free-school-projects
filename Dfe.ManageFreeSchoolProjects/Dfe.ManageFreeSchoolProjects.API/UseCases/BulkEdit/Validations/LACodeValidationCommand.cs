﻿using Dfe.ManageFreeSchoolProjects.API.UseCases.LocalAuthority;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit.Validations
{
    public class LACodeValidationCommand(ILocalAuthorityCache localAuthorityCache) : IValidationCommand<BulkEditDto>
    {
        public ValidationResult Execute(ValidationCommandParameters<BulkEditDto> parameters)
        {
            if(parameters.Value.Length < 3)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Local authority must be 3 numbers or more"
                };
            }

            var isValid = localAuthorityCache.GetLocalAuthorities().Exists(x => x.LACode == parameters.Value);
            
            return new ValidationResult
            {
                IsValid = isValid,
                ErrorMessage = isValid ? null : "Enter an existing local authority"
            };
        }
    }
}
