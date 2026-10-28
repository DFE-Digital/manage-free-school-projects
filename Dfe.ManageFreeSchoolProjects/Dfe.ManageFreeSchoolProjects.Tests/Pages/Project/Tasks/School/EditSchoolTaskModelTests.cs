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

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.School
{
    public class EditSchoolTaskModelTests
    {
        private const string ProjectId = "FS-1";

        private readonly IGetProjectByTaskService _getProjectService = Substitute.For<IGetProjectByTaskService>();
        private readonly IUpdateProjectByTaskService _updateProjectTaskService = Substitute.For<IUpdateProjectByTaskService>();

        private UpdateProjectByTaskRequest? _capturedRequest;

        public EditSchoolTaskModelTests()
        {
            _updateProjectTaskService
                .When(service => service.Execute(Arg.Any<string>(), Arg.Any<UpdateProjectByTaskRequest>()))
                .Do(call => _capturedRequest = call.Arg<UpdateProjectByTaskRequest>());
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
        /// A radio group with nothing selected posts no value at all, so the Required attribute on
        /// the non nullable enum is satisfied by NotSet and the page has to reject it itself.
        /// </summary>
        [Fact]
        public async Task OnPost_WithNoEstablishmentTypeSelected_DoesNotSave()
        {
            var model = BuildModel();
            SetValidAnswers(model);
            model.EstablishmentType = EstablishmentType.NotSet;

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["establishment-type"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the establishment type");
            await _updateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
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
