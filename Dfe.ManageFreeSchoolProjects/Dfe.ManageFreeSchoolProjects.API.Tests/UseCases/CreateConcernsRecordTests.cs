using Dfe.ManageFreeSchoolProjects.API.Factories;
using Dfe.ManageFreeSchoolProjects.API.RequestModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases;
using Dfe.ManageFreeSchoolProjects.Data.Gateways;
using Dfe.ManageFreeSchoolProjects.Data.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases
{
    public class CreateConcernsRecordTests
    {
        [Fact]
        public void ShouldCreateAndReturnAConcernsRecord_WhenGivenAConcernsRecordRequest()
        {
            var concernsRecordGateway = new Mock<IConcernsRecordGateway>();
            var concernsCaseGateway = new Mock<IConcernsCaseGateway>();
            var concernsTypeGateway = new Mock<IConcernsTypeGateway>();
            var concernsRatingGateway = new Mock<IConcernsRatingGateway>();
            var concernsMeansOfReferralGateway = new Mock<IConcernsMeansOfReferralGateway>();

            var createRequest = Builder<ConcernsRecordRequest>.CreateNew().Build();
            var concernsCase = Builder<ConcernsCase>.CreateNew().Build();
            var concernsType = Builder<ConcernsType>.CreateNew().Build();
            var concernsRating = Builder<ConcernsRating>.CreateNew().Build();
            var concernsMeansOfReferral = Builder<ConcernsMeansOfReferral>.CreateNew().Build();

            var createdConcernsRecord = ConcernsRecordFactory.Create(createRequest, concernsCase, concernsType, concernsRating, concernsMeansOfReferral);
            var expected = ConcernsRecordResponseFactory.Create(createdConcernsRecord);

            concernsRecordGateway.Setup(g => g.SaveConcernsCase(It.IsAny<ConcernsRecord>())).Returns(createdConcernsRecord);
            concernsCaseGateway.Setup(g => g.GetConcernsCaseByUrn(It.IsAny<int>(), false)).Returns(concernsCase);
            concernsTypeGateway.Setup(g => g.GetConcernsTypeById(It.IsAny<int>())).Returns(concernsType);
            concernsRatingGateway.Setup(g => g.GetRatingById(It.IsAny<int>())).Returns(concernsRating);
            concernsMeansOfReferralGateway.Setup(g => g.GetMeansOfReferralById(concernsMeansOfReferral.Id)).Returns(concernsMeansOfReferral);

            var useCase = new CreateConcernsRecord(
                concernsRecordGateway.Object,
                concernsCaseGateway.Object,
                concernsTypeGateway.Object,
                concernsRatingGateway.Object,
                concernsMeansOfReferralGateway.Object);

            var result = useCase.Execute(createRequest);
            result.Should().BeEquivalentTo(expected);
        }
    }
}