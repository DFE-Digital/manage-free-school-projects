using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create.Individual
{
    public class ProjectAssignedToTests
    {
        [Fact]
        public void OnGet_WhenUserIsNotProjectRecordCreator_ReturnsUnauthorized()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _, authorised: false);

            var result = model.OnGet();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void OnGet_PopulatesNameAndEmailFromTheCache()
        {
            var cacheItem = new CreateProjectCacheItem
            {
                ProjectType = ProjectType.PresumptionRoute,
                ProjectAssignedToName = "John Smith",
                ProjectAssignedToEmail = "john.smith@education.gov.uk"
            };
            var model = BuildModel(cacheItem, out _);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Name.Should().Be("John Smith");
            model.Email.Should().Be("john.smith@education.gov.uk");
        }

        [Fact]
        public void OnPost_WhenRequiredFieldsMissing_ReturnsPage()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out var cache);
            model.ModelState.AddModelError("name", "Enter the name");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            cache.DidNotReceive().Update(Arg.Any<CreateProjectCacheItem>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("John")]
        public void OnPost_WhenNameIsNotAFullName_AddsError(string? name)
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _);
            model.Name = name;
            model.Email = "john.smith@education.gov.uk";

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["name"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the full name, for example John Smith");
        }

        [Fact]
        public void OnPost_WhenNameContainsNumbers_AddsError()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _);
            model.Name = "John Smith2";
            model.Email = "john.smith@education.gov.uk";

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["name"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Name must not include numbers");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("john.smith@gmail.com")]
        [InlineData("not-an-email")]
        [InlineData("@education.gov.uk")]
        public void OnPost_WhenEmailIsNotAnEducationAddress_AddsError(string? email)
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _);
            model.Name = "John Smith";
            model.Email = email;

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["email"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should()
                .Be("Email address must be in the format firstname.surname@education.gov.uk");
        }

        [Fact]
        public void OnPost_WhenEmailIsLongerThan100Characters_AddsError()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _);
            model.Name = "John Smith";
            model.Email = new string('a', 90) + "@education.gov.uk";

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["email"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Email address must be 100 characters or less");
        }

        [Fact]
        public void OnPost_WhenValid_StoresAssigneeAndRedirectsToCheckYourAnswers()
        {
            var cacheItem = new CreateProjectCacheItem();
            var model = BuildModel(cacheItem, out var cache);
            model.Name = "John Smith";
            model.Email = "john.smith@education.gov.uk";

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(RouteConstants.CreateProjectCheckYourAnswers);
            cacheItem.ProjectAssignedToName.Should().Be("John Smith");
            cacheItem.ProjectAssignedToEmail.Should().Be("john.smith@education.gov.uk");
            cache.Received(1).Update(cacheItem);
        }

        private static ProjectAssignedTo BuildModel(
            CreateProjectCacheItem cacheItem, out ICreateProjectCache cache, bool authorised = true)
        {
            // OnPost derives the back link, which for a new school project needs a faith status.
            cacheItem.FaithStatus = cacheItem.FaithStatus == default ? FaithStatus.None : cacheItem.FaithStatus;

            cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new ProjectAssignedTo(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(authorised)
            };
        }
    }
}
