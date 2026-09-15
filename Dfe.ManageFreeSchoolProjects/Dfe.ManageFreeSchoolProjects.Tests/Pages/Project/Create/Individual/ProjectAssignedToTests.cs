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
        public void OnGet_PopulatesTheEmailFromTheCache()
        {
            var cacheItem = new CreateProjectCacheItem
            {
                ProjectType = ProjectType.PresumptionRoute,
                ProjectAssignedToEmail = "john.smith@education.gov.uk"
            };
            var model = BuildModel(cacheItem, out _);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Email.Should().Be("john.smith@education.gov.uk");
        }

        [Fact]
        public void OnPost_WhenRequiredFieldsMissing_ReturnsPage()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out var cache);
            model.ModelState.AddModelError("email", "Enter the email address");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            cache.DidNotReceive().Update(Arg.Any<CreateProjectCacheItem>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("john.smith@gmail.com")]
        [InlineData("not-an-email")]
        [InlineData("@education.gov.uk")]
        public void OnPost_WhenEmailIsNotAnEducationAddress_AddsError(string? email)
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _);
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
            model.Email = "john.smith@education.gov.uk";

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(RouteConstants.CreateProjectCheckYourAnswers);
            cacheItem.ProjectAssignedToEmail.Should().Be("john.smith@education.gov.uk");
            cache.Received(1).Update(cacheItem);
        }

        /// <summary>
        /// A new school project skips the provisional opening date, so its back link goes to whichever
        /// faith page the user actually saw.
        /// </summary>
        [Theory]
        [InlineData(FaithStatus.None, RouteConstants.CreateFaithStatus)]
        [InlineData(FaithStatus.Designation, RouteConstants.CreateFaithType)]
        [InlineData(FaithStatus.Ethos, RouteConstants.CreateFaithType)]
        public void OnGet_WhenNewSchool_LinksBackToTheFaithPage(FaithStatus faithStatus, string expectedBackLink)
        {
            var model = BuildTestableModel(new CreateProjectCacheItem
            {
                ProjectType = ProjectType.NewSchool,
                FaithStatus = faithStatus
            });

            model.OnGet();

            model.BackLinkValue.Should().Be(expectedBackLink);
        }

        [Fact]
        public void OnGet_WhenNotNewSchool_LinksBackToTheProvisionalOpeningDate()
        {
            var model = BuildTestableModel(new CreateProjectCacheItem
            {
                ProjectType = ProjectType.PresumptionRoute,
                FaithStatus = FaithStatus.None
            });

            model.OnGet();

            model.BackLinkValue.Should().Be(RouteConstants.CreateProjectProvisionalOpeningDate);
        }

        private static ProjectAssignedTo BuildModel(
            CreateProjectCacheItem cacheItem, out ICreateProjectCache cache, bool authorised = true)
        {
            cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new ProjectAssignedTo(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(authorised)
            };
        }

        private static TestableProjectAssignedTo BuildTestableModel(CreateProjectCacheItem cacheItem)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new TestableProjectAssignedTo(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build()
            };
        }

        /// <summary>
        /// BackLink is protected internal, so it can only be read from a derived page model.
        /// </summary>
        private sealed class TestableProjectAssignedTo(ErrorService errorService, ICreateProjectCache createProjectCache)
            : ProjectAssignedTo(errorService, createProjectCache)
        {
            public string BackLinkValue => BackLink;
        }
    }
}
