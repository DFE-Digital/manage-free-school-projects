﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using Dfe.ManageFreeSchoolProjects.API.Tests.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
	public class ProjectTaskApiTests : ApiTestsBase
	{
		public ProjectTaskApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
		{
		}

		[Fact]
		public async Task Get_ProjectByTask_NoDependentDataCreated_Returns_200()
		{
			// Ensures that if the child tables for the tasks are not populated, the api still works
			var project = DatabaseModelBuilder.BuildProject();
			var projectId = project.ProjectStatusProjectId;

			using var context = _testFixture.GetContext();
			context.Kpi.Add(project);

			var tasks = TasksStub.BuildListOfTasks(project.Rid);
			context.Tasks.AddRange(tasks);

			await context.SaveChangesAsync();

			var getProjectByTaskResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/tasks");
			getProjectByTaskResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Fact]
		public async Task Get_ProjectByTask_DoesNotExist_Returns_404()
		{
			var getProjectByTaskResponse = await _client.GetAsync($"/api/v1/client/projects/NotExist/tasks");
			getProjectByTaskResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Patch_SchoolTask_Returns_201()
		{
			var project = DatabaseModelBuilder.BuildProject();
			var projectId = project.ProjectStatusProjectId;

			using var context = _testFixture.GetContext();
			context.Kpi.Add(project);

			var tasks = TasksStub.BuildListOfTasks(project.Rid);
			context.Tasks.AddRange(tasks);

			await context.SaveChangesAsync();

			var request = new UpdateProjectByTaskRequest()
			{
				School = new SchoolTask()
				{
					CurrentFreeSchoolName = "Test High School",
					SchoolType = SchoolType.Mainstream,
					AgeRange = "11-18",
					SchoolPhase = SchoolPhase.Primary,
					Nursery = ClassType.Nursery.No,
					SixthForm = ClassType.SixthForm.No,
					FaithStatus = FaithStatus.NotSet,
					FaithType = FaithType.Other,
					Gender = Gender.Mixed
				}
			};

            var projectResponse = await UpdateProjectTask(projectId, request);

            projectResponse.School.CurrentFreeSchoolName.Should().Be("Test High School");
            projectResponse.School.SchoolType.Should().Be(SchoolType.Mainstream);
            projectResponse.School.SchoolPhase.Should().Be(SchoolPhase.Primary);
            projectResponse.School.AgeRange.Should().Be("11-18");
            projectResponse.School.Nursery.Should().Be(ClassType.Nursery.No);
            projectResponse.School.SixthForm.Should().Be(ClassType.SixthForm.No);
        }

		[Fact]
		public async Task Patch_ConstituencyTask_Returns_201()
		{
			var project = DatabaseModelBuilder.BuildProject();
			var projectId = project.ProjectStatusProjectId;

			using var context = _testFixture.GetContext();
			context.Kpi.Add(project);

			var tasks = TasksStub.BuildListOfTasks(project.Rid);
			context.Tasks.AddRange(tasks);

			await context.SaveChangesAsync();

			const string Battersea = "Battersea";
			const string TeddyBones = "RT Hon Theodore Bones";
			const string MRL = "Monster Raving Loony";
			var request = new UpdateProjectByTaskRequest()
			{
				Constituency = new ConstituencyTask()
				{
					Name = Battersea,
					MPName = TeddyBones,
					Party = MRL,
				}
			};

			var projectResponse = await UpdateProjectTask(projectId, request);

			projectResponse.Constituency.Name.Should().Be(Battersea);
			projectResponse.Constituency.MPName.Should().Be(TeddyBones);
			projectResponse.Constituency.Party.Should().Be(MRL);

		}

		[Fact]
		public async Task Patch_DatesTask_Returns_201()
		{
			var project = DatabaseModelBuilder.BuildProject();
			var projectId = project.ProjectStatusProjectId;

			using var context = _testFixture.GetContext();
			context.Kpi.Add(project);
			await context.SaveChangesAsync();

			var DateTenDaysInFuture = new DateTime().AddDays(10);
			var DateNineDaysInFuture = new DateTime().AddDays(9);

			var request = new UpdateProjectByTaskRequest()
			{
				Dates = new DatesTask()
				{
					DateOfEntryIntoPreopening = DateTenDaysInFuture,
					ProvisionalOpeningDateAgreedWithTrust = DateNineDaysInFuture,
					RealisticYearOfOpening = "2023 2024",
				}
			};

			var projectResponse = await UpdateProjectTask(projectId, request);

			projectResponse.Dates.DateOfEntryIntoPreopening.Should().Be(DateTenDaysInFuture);
			projectResponse.Dates.ProvisionalOpeningDateAgreedWithTrust.Should().Be(DateNineDaysInFuture);
			projectResponse.Dates.RealisticYearOfOpening.Should().Be("2023 2024");
		}

		[Fact]
		public async Task Patch_LocalAuthorityAndRegionTask_Returns_201()
		{
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            await context.SaveChangesAsync();

			var request = new UpdateProjectByTaskRequest()
			{
				RegionAndLocalAuthorityTask = new()
				{
                    LocalAuthority = "LocalAuthority",
                    Region = "Region"
                }
            };

            var projectResponse = await UpdateProjectTask(projectId, request);

			projectResponse.RegionAndLocalAuthority.LocalAuthority.Should().Be("LocalAuthority");
			projectResponse.RegionAndLocalAuthority.Region.Should().Be("Region");
        }

        [Fact]
        public async Task Patch_NewRiskAppraisalMeetingTask_Returns_201()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            await context.SaveChangesAsync();

            var DateTenDaysInFuture = new DateTime().AddDays(10);
            var DateNineDaysInFuture = new DateTime().AddDays(9);

            var request = new UpdateProjectByTaskRequest()
            {
                RiskAppraisalMeeting = new RiskAppraisalMeetingTask()
                {
					ForecastDate = DateTenDaysInFuture,
					ActualDate = DateNineDaysInFuture,
					CommentsOnDecisionToApprove = "CommentsOnDecisionToApprove",
					InitialRiskAppraisalMeetingCompleted= true,
					ReasonNotApplicable= "ReasonNotApplicable"
                }
            };

            var projectResponse = await UpdateProjectTask(projectId, request);

            projectResponse.RiskAppraisalMeeting.ForecastDate.Should().Be(DateTenDaysInFuture);
            projectResponse.RiskAppraisalMeeting.ActualDate.Should().Be(DateNineDaysInFuture);
            projectResponse.RiskAppraisalMeeting.CommentsOnDecisionToApprove.Should().Be("CommentsOnDecisionToApprove");
            projectResponse.RiskAppraisalMeeting.InitialRiskAppraisalMeetingCompleted.Should().Be(true);
            projectResponse.RiskAppraisalMeeting.ReasonNotApplicable.Should().Be("ReasonNotApplicable");
        }

        [Fact]
        public async Task Patch_ExistingRiskAppraisalMeetingTask_Returns_201()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var riskAppraisalMeetingTask = DatabaseModelBuilder.BuildRiskAppraisalMeetingTask(project.Rid);
            context.RiskAppraisalMeetingTask.Add(riskAppraisalMeetingTask);

            await context.SaveChangesAsync();

            var DateTenDaysInFuture = new DateTime().AddDays(10);
            var DateNineDaysInFuture = new DateTime().AddDays(9);

            var request = new UpdateProjectByTaskRequest()
            {
                RiskAppraisalMeeting = new RiskAppraisalMeetingTask()
                {
                    ForecastDate = DateTenDaysInFuture,
                    ActualDate = DateNineDaysInFuture,
                    CommentsOnDecisionToApprove = "CommentsOnDecisionToApprove",
                    InitialRiskAppraisalMeetingCompleted = true,
                    ReasonNotApplicable = "ReasonNotApplicable"
                }
            };

            var projectResponse = await UpdateProjectTask(projectId, request);

            projectResponse.RiskAppraisalMeeting.ForecastDate.Should().Be(DateTenDaysInFuture);
            projectResponse.RiskAppraisalMeeting.ActualDate.Should().Be(DateNineDaysInFuture);
            projectResponse.RiskAppraisalMeeting.CommentsOnDecisionToApprove.Should().Be("CommentsOnDecisionToApprove");
            projectResponse.RiskAppraisalMeeting.InitialRiskAppraisalMeetingCompleted.Should().Be(true);
            projectResponse.RiskAppraisalMeeting.ReasonNotApplicable.Should().Be("ReasonNotApplicable");

			context.RiskAppraisalMeetingTask.Count(r => r.RID == project.Rid).Should().Be(1);
        }

		[Fact]
		public async Task Patch_TrustTask_Returns_201()
		{
            var project = DatabaseModelBuilder.BuildProject();
			var trust = DatabaseModelBuilder.BuildTrust();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
			context.Trust.Add(trust);
            await context.SaveChangesAsync();

			var request = new UpdateProjectByTaskRequest()
			{
				Trust = new TrustTask()
				{
					TRN = trust.TrustRef
				}
			};

            var projectResponse = await UpdateProjectTask(projectId, request);

			projectResponse.Trust.TRN.Should().Be(trust.TrustRef);
			projectResponse.Trust.TrustName.Should().Be(trust.TrustsTrustName);
			projectResponse.Trust.TrustType.Should().Be(trust.TrustsTrustType);
        }

        [Fact]
		public async Task Patch_Task_NoProjectExists_Returns_404()
		{
			var request = new UpdateProjectByTaskRequest()
			{
			};

			var updateTaskResponse = await _client.PatchAsync($"/api/v1/client/projects/NotExist/tasks", request.ConvertToJson());
			updateTaskResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		private async Task<GetProjectByTaskResponse> UpdateProjectTask(string projectId, UpdateProjectByTaskRequest request)
		{
			var updateTaskResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/tasks", request.ConvertToJson());
			updateTaskResponse.StatusCode.Should().Be(HttpStatusCode.OK);

			var getProjectByTaskResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/tasks");
			getProjectByTaskResponse.StatusCode.Should().Be(HttpStatusCode.OK);

			var result = await getProjectByTaskResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<GetProjectByTaskResponse>>();

			return result.Data;
		}
	}
}
