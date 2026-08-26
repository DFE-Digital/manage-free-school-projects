using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.PupilNumbers;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.PupilNumbers;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Dfe.ManageFreeSchoolProjects.API.Constants;
using Microsoft.EntityFrameworkCore;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.School;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals
{
    public interface ICreateProposalService
    {
        Task<CreateProposalResponse> Execute(CreateProposalRequest createRequest);
    }

    public class CreateProposalService : ICreateProposalService
    {
        private readonly MfspContext _context;

        public CreateProposalService(MfspContext context)
        {
            _context = context;
        }

        public async Task<CreateProposalResponse> Execute(CreateProposalRequest createRequest)
        {
            var result = new CreateProposalResponse
            {
                ProjectId = createRequest.ProjectId
            };

            return result;
        }
    }
}
