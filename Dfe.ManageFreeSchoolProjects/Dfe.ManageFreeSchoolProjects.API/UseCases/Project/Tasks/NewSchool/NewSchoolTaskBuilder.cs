using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool
{
    public static class NewSchoolTaskBuilder
    {
        public static NewSchoolTask Build(Kpi kpi)
        {
            return new NewSchoolTask
            {
                NewSchoolSpecificationPublicationDate = kpi.NewSchoolSpecificationPublicationDate,
                NewSchoolClosingDateForProposals = kpi.NewSchoolClosingDateForProposals,
                NewSchoolDecisionMaker = kpi.NewSchoolDecisionMaker,
                NewSchoolDateOfDecision = kpi.NewSchoolDateOfDecision,

                //NewSchoolWasTheDecisionApprovedWithoutConditions = kpi.NewSchoolWasTheDecisionApprovedWithoutConditions,
                //NewSchoolHaveAnyConditionBeenApplied = kpi.NewSchoolHaveAnyConditionBeenApplied,
                NewSchoolDateForConditionsToBeMet = kpi.NewSchoolDateForConditionsToBeMet
            };
        }
    }
}
