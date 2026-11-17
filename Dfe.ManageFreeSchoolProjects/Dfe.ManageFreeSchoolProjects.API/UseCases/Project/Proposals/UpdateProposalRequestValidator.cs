using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using FluentValidation;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public class UpdateProposalRequestValidator : AbstractValidator<UpdateProposalRequest>
    {
        public UpdateProposalRequestValidator()
        {
            RuleFor(x => x.Rid).NotEmpty();
        }
    }
}
