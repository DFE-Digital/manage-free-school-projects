﻿using Azure.Core;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class ProjectPaymentsApiTests : ApiTestsBase
    {
        public ProjectPaymentsApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        private static Fixture _fixture = new();

        [Fact]
        public async Task Get_ProjectPayments_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var getProjectPaymentsResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/payments");
            getProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //Get project payments project not found
        [Fact]
        public async Task Get_ProjectPayments_Returns_404()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = "abcdef";

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var getProjectPaymentsResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/payments");
            getProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        //Update project payment
        [Fact]
        public async Task Update_ProjectPayments_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var request = new Payment();
            request.PaymentIndex = 2;
            request.PaymentScheduleAmount = ParseDecimal("444");
            request.PaymentScheduleDate = payments.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue;
            request.PaymentActualAmount = ParseDecimal(payments.ProjectDevelopmentGrantFundingAmountOf2ndPayment);
            request.PaymentActualDate = payments.ProjectDevelopmentGrantFundingDateOf2ndActualPayment;

            var patchProjectPaymentsResponse = await _client.PutAsync($"/api/v1/client/projects/{projectId}/payments/update", request.ConvertToJson());
            patchProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProjectPaymentsResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/payments");
            getProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var projectPayments = await getProjectPaymentsResponse.Content.ReadFromJsonAsync<ApiListWrapper<ProjectPayments>>();
            projectPayments.Data.Should().Contain(p => p.Payments.First(i => i.PaymentIndex == request.PaymentIndex).PaymentActualAmount == request.PaymentScheduleAmount);

        }

        //Update project payment index out of range
        [Fact]
        public async Task Update_ProjectPayments_ForPaymentIndexOutOfRange_Returns_404()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var request = new Payment();
            request.PaymentIndex = 3;
            request.PaymentScheduleAmount = ParseDecimal(payments.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue);
            request.PaymentScheduleDate = payments.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue;
            request.PaymentActualAmount = ParseDecimal(payments.ProjectDevelopmentGrantFundingAmountOf2ndPayment);
            request.PaymentActualDate = payments.ProjectDevelopmentGrantFundingDateOf2ndActualPayment;

            var patchProjectPaymentsResponse = await _client.PutAsync($"/api/v1/client/projects/{projectId}/payments/update", request.ConvertToJson());
            patchProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        //Add project payment

        [Fact]
        public async Task Add_ProjectPayments_Returns_200()
        {

            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var request = new Payment();
            request.PaymentScheduleAmount = _fixture.Create<decimal>();
            request.PaymentScheduleDate = new DateTime().AddDays(40);
            request.PaymentActualAmount = _fixture.Create<decimal>();
            request.PaymentActualDate = new DateTime().AddDays(40);

            var patchProjectPaymentsResponse = await _client.PostAsync($"/api/v1/client/projects/{projectId}/payments/add", request.ConvertToJson());
            patchProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProjectPaymentsResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/payments");
            getProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var projectPayments = await getProjectPaymentsResponse.Content.ReadFromJsonAsync<ApiListWrapper<ProjectPayments>>();
            projectPayments.Data.Should().Contain(p => p.Payments.First(i => i.PaymentIndex == request.PaymentIndex).PaymentActualAmount == request.PaymentScheduleAmount);

        }
        //Add project payment index out of range

        [Fact]
        public async Task Add_ProjectPayments_WhenPaymentsFull_Returns_404()
        {

            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildAllProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var request = new Payment();
            request.PaymentScheduleAmount = _fixture.Create<decimal>();
            request.PaymentScheduleDate = new DateTime().AddDays(40);
            request.PaymentActualAmount = _fixture.Create<decimal>();
            request.PaymentActualDate = new DateTime().AddDays(40);

            var patchProjectPaymentsResponse = await _client.PostAsync($"/api/v1/client/projects/{projectId}/payments/add", request.ConvertToJson());
            patchProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        //Delete project payment index not found

        [Fact]
        public async Task Delete_ProjectPayments_Returns_200()
        {

            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var paymentIndexToDelete = 2;

            var patchProjectPaymentsResponse = await _client.DeleteAsync($"/api/v1/client/projects/{projectId}/payments/delete/{paymentIndexToDelete}");
            patchProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProjectPaymentsResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/payments");
            getProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var projectPayments = await getProjectPaymentsResponse.Content.ReadFromJsonAsync<ApiListWrapper<ProjectPayments>>();
            projectPayments.Data.Should().NotContain(p => p.Payments.First(i => i.PaymentIndex == paymentIndexToDelete).PaymentIndex == paymentIndexToDelete);
        }
        [Fact]
        public async Task Delete_ProjectPayments_PaymentNotFound_Returns_404()
        {

            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var payments = DatabaseModelBuilder.BuildProjectPayments(project.Rid);
            context.Po.Add(payments);

            await context.SaveChangesAsync();

            var paymentIndexToDelete = 6;

            var patchProjectPaymentsResponse = await _client.DeleteAsync($"/api/v1/client/projects/{projectId}/payments/delete/{paymentIndexToDelete}");
            patchProjectPaymentsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        private static decimal? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return decimal.Parse(value);
        }
    }
}
