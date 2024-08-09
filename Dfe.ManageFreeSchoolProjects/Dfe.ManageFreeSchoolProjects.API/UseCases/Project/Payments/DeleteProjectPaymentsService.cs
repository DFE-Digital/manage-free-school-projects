using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Payments;
public interface IDeleteProjectPaymentsService
{
    Task Execute(string projectId, int paymentIndex);
}
public class DeleteProjectPaymentsService : IDeleteProjectPaymentsService
{
    private readonly MfspContext _context;

    public DeleteProjectPaymentsService(MfspContext context)
    {
        _context = context;
    }

    public async Task Execute(string projectId, int paymentIndex)
    {
        var dbProject = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

        if (dbProject == null)
        {
            throw new NotFoundException($"Project with id {projectId} not found");
        }

        var po = await _context.Po.FirstOrDefaultAsync(p => p.Rid == dbProject.Rid);

        ProjectPaymentsDeleter.Delete(po, paymentIndex);

        await _context.SaveChangesAsync();
    }

        
}

