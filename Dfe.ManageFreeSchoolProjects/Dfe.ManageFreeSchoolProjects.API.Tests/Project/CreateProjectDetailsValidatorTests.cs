using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using FluentAssertions;
using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Project
{
    public class CreateProjectDetailsValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TRN_WhenLocalAuthorityWave_IsNotRequired(string trn)
        {
            var result = Validate(applicationWave: "New School", trn: trn);

            result.Should().NotContain(error => error.PropertyName == nameof(ProjectDetails.TRN));
        }

        [Theory]
        [InlineData("FS - Presumption")]
        [InlineData("Central Route")]
        [InlineData("")]
        [InlineData(null)]
        public void TRN_WhenNotLocalAuthorityWave_IsRequired(string applicationWave)
        {
            var result = Validate(applicationWave: applicationWave, trn: null);

            result.Should().Contain(error => error.PropertyName == nameof(ProjectDetails.TRN));
        }

        [Fact]
        public void TRN_WhenLocalAuthorityWaveAndSupplied_IsStillAccepted()
        {
            var result = Validate(applicationWave: "New School", trn: "TRN0123");

            result.Should().NotContain(error => error.PropertyName == nameof(ProjectDetails.TRN));
        }

        /// <summary>
        /// The condition must scope to the TRN rule only - the other required fields
        /// still apply to a local authority project.
        /// </summary>
        [Fact]
        public void OtherRules_AreUnaffectedByLocalAuthorityWave()
        {
            var result = Validate(applicationWave: "New School", trn: null);

            result.Should().Contain(error => error.PropertyName == nameof(ProjectDetails.ProjectId));
            result.Should().Contain(error => error.PropertyName == nameof(ProjectDetails.CreatedBy));
        }

        private static List<FluentValidation.Results.ValidationFailure> Validate(
            string applicationWave, string trn)
        {
            var details = new ProjectDetails
            {
                ApplicationWave = applicationWave,
                TRN = trn
            };

            return new CreateProjectDetailsValidator().Validate(details).Errors;
        }
    }
}
