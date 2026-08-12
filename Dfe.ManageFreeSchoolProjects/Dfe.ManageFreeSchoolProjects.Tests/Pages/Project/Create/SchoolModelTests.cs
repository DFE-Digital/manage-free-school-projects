using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create
{
    public class SchoolModelTests
    {
        [Theory]
        [InlineData(ProjectType.LocalAuthority, "What is the current working name of the new school?")]
        [InlineData(ProjectType.PresumptionRoute, "What is the current free school name?")]
        [InlineData(ProjectType.CentralRoute, "What is the current free school name?")]
        public void OnGet_SetsQuestionForProjectType(ProjectType projectType, string expectedQuestion)
        {
            var model = BuildModel(new CreateProjectCacheItem { ProjectType = projectType });

            model.OnGet();

            model.SchoolNameQuestion.Should().Be(expectedQuestion);
        }

        [Theory]
        [InlineData(ProjectType.LocalAuthority, "Enter the new school name")]
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
            var model = BuildModel(new CreateProjectCacheItem { ProjectType = ProjectType.LocalAuthority });
            model.School = "   ";

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["school"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the new school name");
        }

        [Fact]
        public void OnPost_WhenSchoolNameProvided_StoresNameAndRedirects()
        {
            var cacheItem = new CreateProjectCacheItem { ProjectType = ProjectType.LocalAuthority };
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

        private static SchoolModel BuildModel(CreateProjectCacheItem cacheItem)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new SchoolModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build()
            };
        }
    }
}
