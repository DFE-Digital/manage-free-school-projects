using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Pages.Project;
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

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project
{
    public class ProjectOverviewModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";

        [Fact]
        public async Task OnGet_LoadsTheOverviewAndTheDecisionTask()
        {
            var overview = new ProjectOverviewResponse();
            var decisionTask = new GetProjectByTaskResponse
            {
                NewSchoolDecision = new NewSchoolDecisionTask
                {
                    NewSchoolDecision = "Approved with conditions"
                }
            };

            var overviewService = BuildOverviewService(overview);
            var taskService = BuildTaskService(decisionTask);

            var model = BuildModel(overviewService, taskService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectOverview.Should().BeSameAs(overview);
            model.DecisionTask.Should().BeSameAs(decisionTask);
            await overviewService.Received(1).Execute(ProjectId);
            await taskService.Received(1).Execute(ProjectId, TaskName.NewSchoolDecision);
        }

        [Fact]
        public async Task OnGet_WhenTheOverviewCannotBeFetched_StillReturnsThePage()
        {
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(Arg.Any<string>())
                .ThrowsAsync(new InvalidOperationException("API down"));

            var taskService = BuildTaskService(new GetProjectByTaskResponse());

            var model = BuildModel(overviewService, taskService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectOverview.Should().BeNull();
            model.DecisionTask.Should().BeNull();
            await taskService.DidNotReceiveWithAnyArgs().Execute(default!, default);
        }

        [Fact]
        public async Task OnGet_WhenTheDecisionTaskCannotBeFetched_StillReturnsThePageWithTheOverview()
        {
            var overview = new ProjectOverviewResponse();
            var taskService = Substitute.For<IGetProjectByTaskService>();
            taskService.Execute(Arg.Any<string>(), Arg.Any<TaskName>())
                .ThrowsAsync(new InvalidOperationException("API down"));

            var model = BuildModel(BuildOverviewService(overview), taskService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectOverview.Should().BeSameAs(overview);
            model.DecisionTask.Should().BeNull();
        }

        [Fact]
        public async Task OnGet_WhenTheDecisionHasNotBeenMade_LeavesTheAnswerEmpty()
        {
            var decisionTask = new GetProjectByTaskResponse
            {
                NewSchoolDecision = new NewSchoolDecisionTask { NewSchoolDecision = null }
            };

            var model = BuildModel(BuildOverviewService(new ProjectOverviewResponse()), BuildTaskService(decisionTask));

            await model.OnGet();

            model.DecisionTask!.NewSchoolDecision.NewSchoolDecision.Should().BeNull();
        }

        private static IGetProjectOverviewService BuildOverviewService(ProjectOverviewResponse overview)
        {
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(Arg.Any<string>()).Returns(overview);

            return overviewService;
        }

        private static IGetProjectByTaskService BuildTaskService(GetProjectByTaskResponse decisionTask)
        {
            var taskService = Substitute.For<IGetProjectByTaskService>();
            taskService.Execute(Arg.Any<string>(), Arg.Any<TaskName>()).Returns(decisionTask);

            return taskService;
        }

        private static ProjectOverviewModel BuildModel(
            IGetProjectOverviewService overviewService, IGetProjectByTaskService taskService)
        {
            return new ProjectOverviewModel(
                overviewService,
                taskService,
                Substitute.For<ILogger<ProjectOverviewModel>>())
            {
                ProjectId = ProjectId,
                PageContext = BuildPageContext()
            };
        }

        private static PageContext BuildPageContext()
        {
            var routeData = new RouteData();
            routeData.Values["projectId"] = ProjectId;

            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                routeData,
                new PageActionDescriptor(),
                new ModelStateDictionary());

            return new PageContext(actionContext);
        }
    }
}
