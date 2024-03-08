﻿using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class ReportApiTests : ApiTestsBase
    {
        public ReportApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        [Fact]
        public async Task Get_AllProjectsExport_Returns_200()
        {
            using var context = _testFixture.GetContext();

            var project = DatabaseModelBuilder.BuildProject();

            context.Kpi.Add(project);

            await context.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/v1/client/reports/all-projects-export");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var fileStreamResult = await response.Content.ReadAsStreamAsync();

            using var memoryStream = new MemoryStream();
            await fileStreamResult.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            // Check we are able to open the excel file
            using SpreadsheetDocument document = SpreadsheetDocument.Open(memoryStream, false);
        }

        [Fact]
        public async Task GetSfaExport_Returns_200()
        {
            using var context = _testFixture.GetContext();

            var project = DatabaseModelBuilder.BuildProject();

            context.Kpi.Add(project);

            await context.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/v1/client/reports/sfa-export");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var fileStreamResult = await response.Content.ReadAsStringAsync();

            // Split by carriage return
            var lines = fileStreamResult.Split(Environment.NewLine).Where(l => l != string.Empty);

            lines.Should().HaveCountGreaterThan(1);

            // Ensure it is a valid csv
            var headerLine = lines.First();
            var expectedColumns = headerLine.Split(",").Length;

            // Check a line has the expected number of columns
            var firstRow = lines.Skip(1).First();
            firstRow.Split(",").Should().HaveCount(expectedColumns);
        }
    }
}
