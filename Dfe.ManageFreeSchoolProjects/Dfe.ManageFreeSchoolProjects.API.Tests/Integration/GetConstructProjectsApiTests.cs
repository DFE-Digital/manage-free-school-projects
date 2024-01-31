﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Construct;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class GetConstructProjectsApiTests : ApiTestsBase
    {
        public GetConstructProjectsApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        [Fact]
        public async Task Get_ReturnsAllProjects_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            var projectWithMinimumFields = DatabaseModelBuilder.BuildProjectMandatoryFieldsOnly();

            var milestone = DatabaseModelBuilder.BuildArticlesOfAssociationTask(project.Rid);
            var pupilNumbersAndCapacity = DatabaseModelBuilder.PupilNumbersAndCapacity(project.Rid);

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            context.Kpi.Add(projectWithMinimumFields);
            context.Milestones.Add(milestone);
            context.Po.Add(pupilNumbersAndCapacity);

            await context.SaveChangesAsync();

            var getConstructProjectsResponse = await _client.GetAsync($"/api/v1/construct/projects");
            getConstructProjectsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await getConstructProjectsResponse.Content.ReadFromJsonAsync<ApiResponseV2<ConstructProjectResponse>>();
            var constructProjects = content.Data;

            constructProjects.Should().HaveCountGreaterThanOrEqualTo(1);
            var selectedProject = constructProjects.Where(x => x.ProjectId == projectId).FirstOrDefault();

            selectedProject.CurrentFreeSchoolName.Should().Be(project.ProjectStatusCurrentFreeSchoolName);
            selectedProject.ProjectId.Should().Be(project.ProjectStatusProjectId);
            selectedProject.ApplicationWave.Should().Be(project.ProjectStatusFreeSchoolApplicationWave);
            selectedProject.ProjectStatus.Should().Be(project.ProjectStatusProjectStatus);
            selectedProject.ProvisionalOpeningDateAgreedWithTrust.Should().Be(project.ProjectStatusProvisionalOpeningDateAgreedWithTrust.Value.Date);
            selectedProject.ActualOpeningDate.Should().Be(project.ProjectStatusActualOpeningDate.Value.Date);
            selectedProject.FreeSchoolPenPortrait.Should().Be(project.ProjectStatusFreeSchoolPenPortrait);
            selectedProject.URN.Should().Be(project.ProjectStatusUrnWhenGivenOne);
            selectedProject.DateSchoolClosed.Should().Be(project.ProjectStatusDateClosed.Value.Date);
            selectedProject.DateOfEntryIntoPreOpening.Should().Be(project.ProjectStatusDateOfEntryIntoPreOpening.Value.Date);
            selectedProject.LocalAuthority.Should().Be(project.LocalAuthority);
            selectedProject.LaesTab.Should().Be(project.SchoolDetailsLaestabWhenGivenOne);
            selectedProject.RSCRegion.Should().Be(project.SchoolDetailsRscRegion);
            selectedProject.NumberOfFormsOfEntry.Should().Be(project.SchoolDetailsNumberOfFormsOfEntry);
            selectedProject.SchoolType.Should().Be(project.SchoolDetailsSchoolTypeMainstreamApEtc);
            selectedProject.SchoolPhase.Should().Be(project.SchoolDetailsSchoolPhasePrimarySecondary);
            selectedProject.AgeRange.Should().Be(project.SchoolDetailsAgeRange);
            selectedProject.Gender.Should().Be(project.SchoolDetailsGender);
            selectedProject.ResidentialOrBoardingProvision.Should().Be(project.SchoolDetailsResidentialOrBoardingProvision);
            selectedProject.ResidentialBoardingProvisionDetails.Should().Be(project.SchoolDetailsDetailsOfResidentialBoardingProvision);
            selectedProject.Nursery.Should().Be(project.SchoolDetailsNursery);
            selectedProject.SixthForm.Should().Be(project.SchoolDetailsSixthForm);
            selectedProject.SixthFormType.Should().Be(project.SchoolDetailsSixthFormType);
            selectedProject.FaithStatus.Should().Be(project.SchoolDetailsFaithStatus);
            selectedProject.FaithType.Should().Be(project.SchoolDetailsFaithType);
            selectedProject.OtherFaithType.Should().Be(project.SchoolDetailsPleaseSpecifyOtherFaithType);
            selectedProject.Specialism.Should().Be(project.SchoolDetailsSpecialism);
            selectedProject.TrustId.Should().Be(project.SchoolDetailsTrustId);
            selectedProject.TrustName.Should().Be(project.SchoolDetailsTrustName);
            selectedProject.SchoolAddress.Should().Be(project.KeyContactsSchoolAddress);
            selectedProject.Postcode.Should().Be(project.KeyContactsPostcode);
            selectedProject.FSGLeadContact.Should().Be(project.KeyContactsFsgLeadContact);
            selectedProject.RealisticYearofOpening.Should().Be(project.ProjectStatusRealisticYearOfOpening);
            selectedProject.MemberOfParliament.Should().Be(project.SchoolDetailsConstituencyMp);
            selectedProject.GeographicalRegion.Should().Be(project.SchoolDetailsGeographicalRegion);

            selectedProject.KickOfMeetingHeldDate.Should().Be(milestone.FsgPreOpeningMilestonesKickOffMeetingHeldActualDate.Value.Date);
            selectedProject.FAActualCompletionDate.Should().Be(milestone.FsgPreOpeningMilestonesFaActualDateOfCompletion.Value.Date);
            selectedProject.FAForecastDate.Should().Be(milestone.FsgPreOpeningMilestonesFaForecastDate.Value.Date);
            selectedProject.NumberOfPupil.Should().Be(pupilNumbersAndCapacity.PupilNumbersAndCapacityTotalOfCapacityTotals);

            // Ensure a project with minimum fields is returned
            // This makes sure that any of the optional join tables to not remove it from the list
            var selectedProjectWithMinimumFields = constructProjects.Where(x => x.ProjectId == projectWithMinimumFields.ProjectStatusProjectId).FirstOrDefault();
            selectedProjectWithMinimumFields.CurrentFreeSchoolName.Should().Be(projectWithMinimumFields.ProjectStatusCurrentFreeSchoolName);
        }
    }
}
