﻿using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Tasks;


public interface ICreateTasksService
{
    Task Execute(string projectId);
}

public class CreateTasksService : ICreateTasksService
{
    private readonly MfspContext _context; 
    
    public CreateTasksService(MfspContext context)
    {
        _context = context;
    }

    public async Task Execute(string projectId)
    {
        var kpi = await _context.Kpi.SingleOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);
        await _context.Tasks.AddRangeAsync(CreateTasks(kpi.Rid));
        await _context.SaveChangesAsync();
    }
    
    private IEnumerable<Data.Entities.Existing.Tasks> CreateTasks(string kpiRid)
    {
        return new List<Data.Entities.Existing.Tasks>()
        {
            new() { Rid = kpiRid, TaskName = TaskName.School, Status = Status.NotStarted  },
            new() { Rid = kpiRid, TaskName = TaskName.Construction, Status = Status.NotStarted },
            new() { Rid = kpiRid, TaskName = TaskName.Dates, Status = Status.NotStarted },
            new() { Rid = kpiRid, TaskName = TaskName.RiskAppraisal, Status = Status.NotStarted },
        };
    }
}