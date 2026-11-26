using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.School;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.School
{
    public class EditSchoolTaskModelTests
    {
        private const string ProjectId = "FS-1";

        private readonly IGetProjectByTaskService _getProjectService = Substitute.For<IGetProjectByTaskService>();
        private readonly IGetProjectOverviewService _getProjectOverviewService = Substitute.For<IGetProjectOverviewService>();
        private readonly IUpdateProjectByTaskService _updateProjectTaskService = Substitute.For<IUpdateProjectByTaskService>();

        private UpdateProjectByTaskRequest? _capturedRequest;

        public EditSchoolTaskModelTests()
        {
            _updateProjectTaskService
                .When(service => service.Execute(Arg.Any<string>(), Arg.Any<UpdateProjectByTaskRequest>()))
                .Do(call => _capturedRequest = call.Arg<UpdateProjectByTaskRequest>());

            ReturnsProjectType("New School");
        }

        /// <summary>
        /// The view only asks for the establishment type on a new school, so the page has to expose
        /// the project type it got from the overview.
        /// </summary>
        [Theory]
        [InlineData("New School")]
        [InlineData("Presumption")]
        [InlineData("Central Route")]
        public async Task OnGet_SetsTheProjectTypeFromTheProjectOverview(string projectType)
        {
            ReturnsSchoolTask(new SchoolTask { SchoolType = SchoolType.Mainstream });
            ReturnsProjectType(projectType);
            var model = BuildModel();

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectType.Should().Be(projectType);
            await _getProjectOverviewService.Received(1).Execute(ProjectId);
        }

        /// <summary>
        /// The project type only decides whether the establishment type is asked, so a failed
        /// overview lookup still shows the rest of the school task.
        /// </summary>
        [Fact]
        public async Task OnGet_WhenTheOverviewCannotBeFetched_StillLoadsTheSchoolTask()
        {
            ReturnsSchoolTask(new SchoolTask
            {
                CurrentFreeSchoolName = "Test High School",
                SchoolType = SchoolType.Mainstream
            });
            _getProjectOverviewService.Execute(Arg.Any<string>())
                .ThrowsAsync(new InvalidOperationException("API down"));
            var model = BuildModel();

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectType.Should().BeNull();
            model.CurrentFreeSchoolName.Should().Be("Test High School");
            model.SchoolType.Should().Be(SchoolType.Mainstream);
        }

        [Fact]
        public async Task OnGet_PopulatesTheEstablishmentTypeFromTheProject()
        {
            ReturnsSchoolTask(new SchoolTask
            {
                CurrentFreeSchoolName = "Test High School",
                SchoolType = SchoolType.Mainstream,
                EstablishmentType = EstablishmentType.AcademySponsorLed
            });
            var model = BuildModel();

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.EstablishmentType.Should().Be(EstablishmentType.AcademySponsorLed);
        }

        /// <summary>
        /// Projects that predate the question have nothing stored, so the radios open with nothing
        /// selected rather than with the first option.
        /// </summary>
        [Fact]
        public async Task OnGet_WhenTheProjectHasNoEstablishmentType_LeavesItUnset()
        {
            ReturnsSchoolTask(new SchoolTask
            {
                CurrentFreeSchoolName = "Test High School",
                SchoolType = SchoolType.Mainstream
            });
            var model = BuildModel();

            await model.OnGet();

            model.EstablishmentType.Should().Be(EstablishmentType.NotSet);
        }

        [Fact]
        public async Task OnPost_SendsTheEstablishmentTypeThatWasSelected()
        {
            var model = BuildModel();
            SetValidAnswers(model);
            model.EstablishmentType = EstablishmentType.PupilReferralUnit;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.ViewSchoolTask, ProjectId));
            _capturedRequest!.School.EstablishmentType.Should().Be(EstablishmentType.PupilReferralUnit);
        }

        /// <summary>
        /// The establishment type is optional, so leaving the radios unanswered still saves the
        /// task, with NotSet sent for the establishment type.
        /// </summary>
        [Fact]
        public async Task OnPost_WithNoEstablishmentTypeSelected_StillSaves()
        {
            var model = BuildModel();
            SetValidAnswers(model);
            model.EstablishmentType = EstablishmentType.NotSet;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.ViewSchoolTask, ProjectId));
            model.ModelState.ContainsKey("establishment-type").Should().BeFalse();
            _capturedRequest!.School.EstablishmentType.Should().Be(EstablishmentType.NotSet);
        }

        [Fact]
        public async Task OnPost_WhenValidationFails_DoesNotUpdateAndKeepsTheEstablishmentTypeJustPosted()
        {
            var model = BuildModel();
            SetValidAnswers(model);
            model.EstablishmentType = EstablishmentType.CommunitySchool;
            model.ModelState.AddModelError("school-type", "Enter the school type");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.EstablishmentType.Should().Be(EstablishmentType.CommunitySchool);
            await _updateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        /// <summary>
        /// The project type is not posted, so without fetching it again a new school would lose
        /// the establishment type radios, and the answer with them, whenever validation fails.
        /// </summary>
        [Fact]
        public async Task OnPost_WhenValidationFails_FetchesTheProjectTypeSoTheEstablishmentTypeIsRedisplayed()
        {
            var model = BuildModel();
            SetValidAnswers(model);
            model.ModelState.AddModelError("school-type", "Enter the school type");

            await model.OnPost();

            model.ProjectType.Should().Be("New School");
            await _getProjectOverviewService.Received(1).Execute(ProjectId);
        }

        [Fact]
        public async Task OnPost_WhenValid_DoesNotFetchTheProjectType()
        {
            var model = BuildModel();
            SetValidAnswers(model);

            await model.OnPost();

            await _getProjectOverviewService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        /// <summary>
        /// Projects that are not new schools are never asked the question, so nothing is posted for
        /// it and the task is saved with the establishment type unset.
        /// </summary>
        [Fact]
        public async Task OnPost_ForAProjectThatIsNotANewSchool_SavesWithoutAnEstablishmentType()
        {
            ReturnsProjectType("Presumption");
            var model = BuildModel();
            SetValidAnswers(model);
            model.EstablishmentType = default;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            _capturedRequest!.School.EstablishmentType.Should().Be(EstablishmentType.NotSet);
        }

        /// <summary>
        /// NotSet exists so an unanswered question can be told apart from an answered one, so it is
        /// never offered as a radio.
        /// </summary>
        [Fact]
        public void TheEstablishmentTypeOptions_DoNotIncludeNotSet()
        {
            var model = BuildModel();

            model.EstablishmentTypes.Should().NotContain(EstablishmentType.NotSet);
            model.EstablishmentTypes.Should().HaveCount(Enum.GetValues<EstablishmentType>().Length - 1);
        }

        /// <summary>
        /// The radio labels are the enum descriptions, so a member without one would be shown to the
        /// user as its C# name.
        /// </summary>
        [Fact]
        public void EveryEstablishmentTypeOption_HasALabelToDisplay()
        {
            var model = BuildModel();

            foreach (var establishmentType in model.EstablishmentTypes)
            {
                establishmentType.ToDescriptionOrEmpty()
                    .Should().NotBeEmpty($"{establishmentType} needs a description to label its radio");
            }
        }

        private void ReturnsProjectType(string projectType)
        {
            _getProjectOverviewService.Execute(Arg.Any<string>())
                .Returns(new ProjectOverviewResponse { ProjectType = projectType });
        }

        private void ReturnsSchoolTask(SchoolTask school)
        {
            _getProjectService.Execute(Arg.Any<string>(), Arg.Any<TaskName>())
                .Returns(new GetProjectByTaskResponse { School = school });
        }

        /// <summary>
        /// The answers the page needs before it will post, so each test only has to set the one it
        /// is about.
        /// </summary>
        private static void SetValidAnswers(EditSchoolTaskModel model)
        {
            model.CurrentFreeSchoolName = "Test High School";
            model.SchoolType = SchoolType.Mainstream;
            model.EstablishmentType = EstablishmentType.CommunitySchool;
            model.SchoolPhase = SchoolPhase.Primary;
            model.AgeRange = "11-18";
            model.Gender = Gender.Mixed;
            model.Nursery = ClassType.Nursery.No;
            model.SixthForm = ClassType.SixthForm.No;
            model.AlternativeProvision = ClassType.AlternativeProvision.No;
            model.SpecialEducationNeeds = ClassType.SpecialEducationNeeds.No;
            model.ResidentialOrBoarding = ClassType.ResidentialOrBoarding.No;
            model.FaithStatus = FaithStatus.None;
            model.FaithType = FaithType.None;
        }

        private EditSchoolTaskModel BuildModel()
        {
            return new EditSchoolTaskModel(
                _getProjectService,
                _getProjectOverviewService,
                _updateProjectTaskService,
                Substitute.For<ILogger<EditSchoolTaskModel>>(),
                new ErrorService())
            {
                ProjectId = ProjectId,
                PageContext = BuildPageContext()
            };
        }

        private static PageContext BuildPageContext()
        {
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new PageActionDescriptor(),
                new ModelStateDictionary());

            return new PageContext(actionContext);
        }
    }
}
