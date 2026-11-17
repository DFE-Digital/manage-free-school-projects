using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecision;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Tasks.NewSchool
{
    /// <summary>
    /// The update task services only copy their own task off the request onto the KPI row the
    /// caller already loaded, so they can be exercised against a bare entity with no database.
    /// Every service runs on every update, which is why the "task not on the request" case matters:
    /// it is what stops one task's save from blanking another task's columns.
    /// </summary>
    public class NewSchoolUpdateTaskServiceTests
    {
        [Fact]
        public async Task DecisionService_StoresTheDecisionAndItsConditions()
        {
            var kpi = new Kpi();

            await new UpdateNewSchoolDecisionTaskService().Update(BuildParameters(kpi, new NewSchoolDecisionTask
            {
                NewSchoolDecision = "Approved with conditions",
                NewSchoolDecisionCondition = "Planning permission required",
                ProposalId = "RID-1"
            }));

            kpi.NewSchoolDecision.Should().Be("Approved with conditions");
            kpi.NewSchoolDecisionCondition.Should().Be("Planning permission required");
        }

        /// <summary>
        /// Choosing "Approved without conditions" sends an empty condition, which has to overwrite
        /// any conditions stored by an earlier save rather than leave them behind.
        /// </summary>
        [Fact]
        public async Task DecisionService_WhenTheDecisionHasNoConditions_ClearsTheStoredConditions()
        {
            var kpi = new Kpi
            {
                NewSchoolDecision = "Approved with conditions",
                NewSchoolDecisionCondition = "Planning permission required"
            };

            await new UpdateNewSchoolDecisionTaskService().Update(BuildParameters(kpi, new NewSchoolDecisionTask
            {
                NewSchoolDecision = "Approved without conditions",
                NewSchoolDecisionCondition = string.Empty,
                ProposalId = "RID-1"
            }));

            kpi.NewSchoolDecision.Should().Be("Approved without conditions");
            kpi.NewSchoolDecisionCondition.Should().BeEmpty();
        }

        [Fact]
        public async Task DecisionService_WhenTheRequestIsForAnotherTask_LeavesTheDecisionAlone()
        {
            var kpi = new Kpi
            {
                NewSchoolDecision = "Approved with conditions",
                NewSchoolDecisionCondition = "Planning permission required"
            };

            await new UpdateNewSchoolDecisionTaskService().Update(BuildParameters(kpi, decisionTask: null));

            kpi.NewSchoolDecision.Should().Be("Approved with conditions");
            kpi.NewSchoolDecisionCondition.Should().Be("Planning permission required");
        }

        private static UpdateTaskServiceParameters BuildParameters(Kpi kpi, NewSchoolDecisionTask decisionTask) =>
            new()
            {
                ProjectId = "NEW-SCHOOL-1",
                Kpi = kpi,
                Request = new UpdateProjectByTaskRequest { NewSchoolDecision = decisionTask }
            };
    }
}
