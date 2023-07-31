﻿using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public interface ICreateProjectService
    {
        Task<long> CreateProject(string ProjectID, string SchoolName, string ApplicationNumber, string ApplicationWave, string CreatedBy);
    }
}
