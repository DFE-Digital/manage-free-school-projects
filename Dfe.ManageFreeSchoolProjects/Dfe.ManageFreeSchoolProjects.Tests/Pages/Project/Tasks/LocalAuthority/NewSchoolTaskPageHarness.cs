using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    /// <summary>
    /// The seven new school task pages take the same three collaborators and follow the same
    /// "send the answer, then set the task status" post flow, so the substitutes and the requests
    /// they receive are set up once here rather than repeated in every page's test class.
    /// </summary>
    internal sealed class NewSchoolTaskPageHarness
    {
        public IGetProjectByTaskService GetProjectService { get; } = Substitute.For<IGetProjectByTaskService>();
        public IUpdateProjectByTaskService UpdateProjectTaskService { get; } = Substitute.For<IUpdateProjectByTaskService>();
        public IUpdateTaskStatusService UpdateTaskStatusService { get; } = Substitute.For<IUpdateTaskStatusService>();
        public ErrorService ErrorService { get; } = new ErrorService();

        public UpdateProjectByTaskRequest? CapturedRequest { get; private set; }
        public UpdateTaskStatusRequest? CapturedStatusRequest { get; private set; }

        public NewSchoolTaskPageHarness()
        {
            UpdateProjectTaskService
                .When(service => service.Execute(Arg.Any<string>(), Arg.Any<UpdateProjectByTaskRequest>()))
                .Do(call => CapturedRequest = call.Arg<UpdateProjectByTaskRequest>());

            UpdateTaskStatusService
                .When(service => service.Execute(Arg.Any<string>(), Arg.Any<UpdateTaskStatusRequest>()))
                .Do(call => CapturedStatusRequest = call.Arg<UpdateTaskStatusRequest>());
        }

        public NewSchoolTaskPageHarness ReturnsProject(GetProjectByTaskResponse project)
        {
            GetProjectService.Execute(Arg.Any<string>(), Arg.Any<TaskName>()).Returns(project);
            return this;
        }

        public NewSchoolTaskPageHarness GetFailsWith(Exception exception)
        {
            GetProjectService.Execute(Arg.Any<string>(), Arg.Any<TaskName>()).ThrowsAsync(exception);
            return this;
        }

        public NewSchoolTaskPageHarness UpdateFailsWith(Exception exception)
        {
            UpdateProjectTaskService
                .When(service => service.Execute(Arg.Any<string>(), Arg.Any<UpdateProjectByTaskRequest>()))
                .Do(_ => throw exception);
            return this;
        }

        public static PageContext BuildPageContext()
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
