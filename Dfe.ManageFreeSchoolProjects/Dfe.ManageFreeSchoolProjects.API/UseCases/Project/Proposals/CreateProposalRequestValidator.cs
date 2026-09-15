using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using FluentValidation;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public class CreateProposalRequestValidator : AbstractValidator<CreateProposalRequest>
    {
        public CreateProposalRequestValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
        }
    }
}
