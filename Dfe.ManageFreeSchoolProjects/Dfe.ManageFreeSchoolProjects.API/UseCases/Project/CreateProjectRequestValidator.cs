using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using FluentValidation;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleForEach(x => x.Projects).SetValidator(new CreateProjectDetailsValidator());
        }
    }

    public class CreateProjectDetailsValidator : AbstractValidator<ProjectDetails>
    {
        public CreateProjectDetailsValidator()
        {
            // Local authority projects are not attached to a trust, so they have no TRN to supply.
            RuleFor(x => x.TRN).NotEmpty().When(x => x.ApplicationWave != "LocalAuthority");
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
