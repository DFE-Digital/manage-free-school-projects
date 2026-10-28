using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Extensions;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.School
{
    public class UpdateSchoolTaskService : IUpdateTaskService
    {
        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.School;
            var dbKpi = parameters.Kpi;

            if (task == null)
            {
                return;
            }

            // Faith status is persisted by name, not description, as the descriptions are radio
            // button hints and cannot be read back into the enum. See EnumParsers.ParseFaithStatus.
            var faithStatus = task.FaithStatus == FaithStatus.NotSet ? string.Empty : ProjectMapper.ToFaithStatus(task.FaithStatus);
            var faithType = task.FaithType == FaithType.NotSet ? string.Empty : task.FaithType.ToDescription();
            var gender = task.Gender == Gender.NotSet ? string.Empty : task.Gender.ToDescription();

            if (dbKpi.ProjectStatusCurrentFreeSchoolName != task.CurrentFreeSchoolName)
            {
                dbKpi.ProjectStatusPreviousFreeSchoolName = dbKpi.ProjectStatusCurrentFreeSchoolName;
                dbKpi.ProjectStatusHasTheFreeSchoolChangedItsName = "Yes";
            }

            dbKpi.ProjectStatusCurrentFreeSchoolName = task.CurrentFreeSchoolName;
            dbKpi.SchoolDetailsSchoolTypeMainstreamApEtc = ProjectMapper.ToSchoolType(task.SchoolType, dbKpi.ProjectStatusFreeSchoolApplicationWave);
            dbKpi.SchoolDetailsSchoolPhasePrimarySecondary = ProjectMapper.ToSchoolPhase(task.SchoolPhase);
            dbKpi.SchoolDetailsGender = gender;
            dbKpi.SchoolDetailsAgeRange = task.AgeRange;
            dbKpi.SchoolDetailsNursery = task.Nursery.ToString();
            dbKpi.SchoolDetailsSixthForm = task.SixthForm.ToString();
            dbKpi.SchoolDetailsAlternativeProvision = task.AlternativeProvision.ToString();
            dbKpi.SchoolDetailsSpecialEducationNeeds = task.SpecialEducationNeeds.ToString();
            dbKpi.SchoolDetailsSpecialistResourceProvision = BuildSpecialistResourceProvision
                                                            .GetLegacySpecialistResourceProvision(task.SpecialEducationNeeds,
                                                                                                  task.AlternativeProvision);
            dbKpi.SchoolDetailsResidentialOrBoardingProvision = task.ResidentialOrBoarding.ToString(); 

            dbKpi.SchoolDetailsFaithStatus = faithStatus;
            dbKpi.SchoolDetailsFaithType = faithType;
            dbKpi.SchoolDetailsPleaseSpecifyOtherFaithType = task.OtherFaithType;
            dbKpi.SchoolDetailsNumberOfFormsOfEntry = task.FormsOfEntry;
        }
    }
}
