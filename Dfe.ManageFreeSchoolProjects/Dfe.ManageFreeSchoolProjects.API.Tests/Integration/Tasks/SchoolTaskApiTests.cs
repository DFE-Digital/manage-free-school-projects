﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using Dfe.ManageFreeSchoolProjects.API.Tests.Utils;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration.Tasks
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class SchoolTaskApiTests : ApiTestsBase
    {
        public SchoolTaskApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
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
                    Gender = Gender.Mixed,
                    FormsOfEntry = "3"
                }
            };

            var projectResponse = await _client.UpdateProjectTask(projectId, request, TaskName.School.ToString());

            projectResponse.School.CurrentFreeSchoolName.Should().Be("Test High School");
            projectResponse.School.SchoolType.Should().Be(SchoolType.Mainstream);
            projectResponse.School.SchoolPhase.Should().Be(SchoolPhase.Primary);
            projectResponse.School.AgeRange.Should().Be("11-18");
            projectResponse.School.Nursery.Should().Be(ClassType.Nursery.No);
            projectResponse.School.SixthForm.Should().Be(ClassType.SixthForm.No);
            projectResponse.SchoolName.Should().Be("Test High School");
            projectResponse.School.FormsOfEntry.Should().Be("3");
        }
    }
}