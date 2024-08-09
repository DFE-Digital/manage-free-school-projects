
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.PaymentSchedule;
using Dfe.ManageFreeSchoolProjects.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Payments;
public interface IGetProjectPaymentsService
{
    Task<ProjectPayments> Execute(string projectId);
}
public class GetProjectPaymentsService : IGetProjectPaymentsService
{
    private readonly MfspContext _context;

    public GetProjectPaymentsService(MfspContext context)
    {
        _context = context;
    }

    public async Task<ProjectPayments> Execute(string projectId)
    {
        var dbProject = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

        if (dbProject == null)
        {
            throw new NotFoundException($"Project with id {projectId} not found");
        }

        var basequery = _context.Kpi.Where(kpi => kpi.ProjectStatusProjectId == projectId);

        var result = await (from kpi in basequery
                            join po in _context.Po on kpi.Rid equals po.Rid into joinedPO
                            from po in joinedPO.DefaultIfEmpty()
                            select new ProjectPayments()
                            {
                                ProjectId = projectId,
                                Payments = GetProjectPaymentsBuilder.Build(po)
                            }).FirstOrDefaultAsync();

        return result ?? new ProjectPayments() { ProjectId = projectId, Payments = new List<Payment>() };
    }
}

