using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolClosingDateForProposals;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolConditions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateForConditionsToBeMet;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateOfDecision;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecision;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecisionMaker;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolSpecificationPublicationDate;
using System;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Tasks.NewSchool
{
    /// <summary>
    /// Each new school get task service projects one KPI row into the task it owns. The tests cover
    /// both the populated projection and the "no project matched" case, which is what the client
    /// sees for a project id that does not exist.
    /// </summary>
    public class NewSchoolGetTaskServiceTests
    {
        private static readonly DateTime PublicationDate = new(2026, 3, 20);
        private static readonly DateTime ClosingDate = new(2026, 5, 1);
        private static readonly DateTime DecisionDate = new(2026, 6, 12);
        private static readonly DateTime ConditionsDate = new(2026, 9, 30);

        [Fact]
        public async Task ConditionsService_ReturnsTheStoredConditions()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(kpi =>
            {
                kpi.NewSchoolConditions = "Yes";
                kpi.NewSchoolConditionsDescription = "Planning permission required";
            });

            var result = await new GetNewSchoolConditionsTaskService().Get(harness.Parameters);

            result.NewSchoolConditions.NewSchoolConditions.Should().Be("Yes");
            result.NewSchoolConditions.NewSchoolConditionsDescription.Should().Be("Planning permission required");
        }

        [Fact]
        public async Task ConditionsService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolConditionsTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DecisionService_ReturnsTheStoredDecision()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(
                kpi => kpi.NewSchoolDecision = "Approved with conditions");

            var result = await new GetNewSchoolDecisionTaskService().Get(harness.Parameters);

            result.NewSchoolDecision.NewSchoolDecision.Should().Be("Approved with conditions");
        }

        [Fact]
        public async Task DecisionService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolDecisionTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DecisionMakerService_ReturnsTheStoredDecisionMaker()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(
                kpi => kpi.NewSchoolDecisionMaker = "Local authority");

            var result = await new GetNewSchoolDecisionMakerTaskService().Get(harness.Parameters);

            result.NewSchoolDecisionMaker.NewSchoolDecisionMaker.Should().Be("Local authority");
        }

        [Fact]
        public async Task DecisionMakerService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolDecisionMakerTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }

        [Fact]
        public async Task SpecificationPublicationDateService_ReturnsTheStoredDate()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(
                kpi => kpi.NewSchoolSpecificationPublicationDate = PublicationDate);

            var result = await new GetNewSchoolSpecificationPublicationDateTaskService().Get(harness.Parameters);

            result.NewSchoolSpecificationPublicationDate.NewSchoolSpecificationPublicationDate
                .Should().Be(PublicationDate);
        }

        [Fact]
        public async Task SpecificationPublicationDateService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolSpecificationPublicationDateTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ClosingDateForProposalsService_ReturnsTheStoredDate()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(
                kpi => kpi.NewSchoolClosingDateForProposals = ClosingDate);

            var result = await new GetNewSchoolClosingDateForProposalsTaskService().Get(harness.Parameters);

            result.NewSchoolClosingDateForProposals.NewSchoolClosingDateForProposals.Should().Be(ClosingDate);
        }

        [Fact]
        public async Task ClosingDateForProposalsService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolClosingDateForProposalsTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DateOfDecisionService_ReturnsTheStoredDate()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(
                kpi => kpi.NewSchoolDateOfDecision = DecisionDate);

            var result = await new GetNewSchoolDateOfDecisionTaskService().Get(harness.Parameters);

            result.NewSchoolDateOfDecision.NewSchoolDateOfDecision.Should().Be(DecisionDate);
        }

        [Fact]
        public async Task DateOfDecisionService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolDateOfDecisionTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DateForConditionsToBeMetService_ReturnsTheStoredDate()
        {
            using var harness = NewSchoolTaskQueryHarness.WithKpi(
                kpi => kpi.NewSchoolDateForConditionsToBeMet = ConditionsDate);

            var result = await new GetNewSchoolDateForConditionsToBeMetTaskService().Get(harness.Parameters);

            result.NewSchoolDateForConditionsToBeMet.NewSchoolDateForConditionsToBeMet
                .Should().Be(ConditionsDate);
        }

        [Fact]
        public async Task DateForConditionsToBeMetService_WhenNoProjectMatches_ReturnsNull()
        {
            using var harness = NewSchoolTaskQueryHarness.WithNoMatchingKpi();

            var result = await new GetNewSchoolDateForConditionsToBeMetTaskService().Get(harness.Parameters);

            result.Should().BeNull();
        }
    }
}
