using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Proposals
{
    public class UpdateProposalRequestValidatorTests
    {
        [Fact]
        public void Validate_WhenTheRidIsPresent_Passes()
        {
            var result = new UpdateProposalRequestValidator().Validate(new UpdateProposalRequest
            {
                Rid = "RID-1",
                Proposer = ProposalProposer.Diocese
            });

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WithoutARidThereIsNothingToUpdate_SoItFails(string rid)
        {
            var result = new UpdateProposalRequestValidator().Validate(new UpdateProposalRequest { Rid = rid });

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be(nameof(UpdateProposalRequest.Rid));
        }
    }
}
