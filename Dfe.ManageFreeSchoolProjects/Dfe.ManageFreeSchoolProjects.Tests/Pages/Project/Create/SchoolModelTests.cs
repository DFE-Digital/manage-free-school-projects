using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create
{
    public class SchoolModelTests
    {
        [Theory]
        [InlineData(ProjectType.NewSchool, "What is the current working name of the new school?")]
        [InlineData(ProjectType.PresumptionRoute, "What is the current free school name?")]
        [InlineData(ProjectType.CentralRoute, "What is the current free school name?")]
        public void OnGet_SetsQuestionForProjectType(ProjectType projectType, string expectedQuestion)
        {
            var model = BuildModel(new CreateProjectCacheItem { ProjectType = projectType });

            model.OnGet();

            model.SchoolNameQuestion.Should().Be(expectedQuestion);
        }

        [Theory]
        [InlineData(ProjectType.NewSchool, "Enter the new school name")]
        [InlineData(ProjectType.PresumptionRoute, "Enter the current free school name")]
        [InlineData(ProjectType.CentralRoute, "Enter the current free school name")]
        public void OnPost_WhenSchoolNameMissing_AddsErrorForProjectType(ProjectType projectType, string expectedError)
        {
            var model = BuildModel(new CreateProjectCacheItem { ProjectType = projectType });
            model.School = null;

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["school"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(expectedError);
        }

        [Fact]
        public void OnPost_WhenSchoolNameIsWhitespace_AddsError()
        {
            var model = BuildModel(new CreateProjectCacheItem { ProjectType = ProjectType.NewSchool });
            model.School = "   ";

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["school"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the new school name");
        }

        [Fact]
        public void OnPost_WhenSchoolNameProvided_StoresNameAndRedirects()
        {
            var cacheItem = new CreateProjectCacheItem { ProjectType = ProjectType.NewSchool };
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            var model = new SchoolModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(),
                School = "Test School"
            };

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            cacheItem.SchoolName.Should().Be("Test School");
            cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public void OnGet_WhenUserIsNotProjectRecordCreator_ReturnsUnauthorized()
        {
            var model = BuildModel(new CreateProjectCacheItem(), authorised: false);

            var result = model.OnGet();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void OnGet_PopulatesSchoolNameFromTheCache()
        {
            var model = BuildModel(new CreateProjectCacheItem
            {
                ProjectType = ProjectType.NewSchool,
                SchoolName = "Test School"
            });

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.School.Should().Be("Test School");
            model.SchoolNameQuestion.Should().Be("What is the current working name of the new school?");
        }

        [Theory]
        [InlineData(false, RouteConstants.CreateProjectRegion)]
        [InlineData(true, RouteConstants.CreateProjectCheckYourAnswers)]
        public void OnPost_WhenValid_RedirectsToNextPage(bool reachedCheckYourAnswers, string expectedRoute)
        {
            var model = BuildModel(new CreateProjectCacheItem
            {
                ProjectType = ProjectType.NewSchool,
                ReachedCheckYourAnswers = reachedCheckYourAnswers
            });
            model.School = "Test School";

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(expectedRoute);
        }

        [Theory]
        [InlineData("School <name>", "School name must not include special characters other than , ( ) '")]
        public void SchoolName_RejectsSpecialCharacters(string school, string expectedError)
        {
            var model = BuildModel(new CreateProjectCacheItem());
            model.School = school;

            var results = Validate(model);

            results.Select(r => r.ErrorMessage).Should().ContainSingle()
                .Which.Should().Be(expectedError);
        }

        [Fact]
        public void SchoolName_RejectsNamesLongerThan100Characters()
        {
            var model = BuildModel(new CreateProjectCacheItem());
            model.School = new string('a', 101);

            var results = Validate(model);

            results.Should().ContainSingle()
                .Which.ErrorMessage.Should().Contain("100");
        }

        private static List<ValidationResult> Validate(SchoolModel model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);

            return results;
        }

        private static SchoolModel BuildModel(CreateProjectCacheItem cacheItem, bool authorised = true)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new SchoolModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(authorised)
            };
        }
    }
}
