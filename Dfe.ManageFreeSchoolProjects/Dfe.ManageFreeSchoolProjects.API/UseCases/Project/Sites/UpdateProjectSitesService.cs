﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites
{
    public interface IUpdateProjectSitesService
    {
        public Task Execute(string projectId, UpdateProjectSitesRequest request, ProjectSiteType siteType);
    }

    public class UpdateProjectSitesService : IUpdateProjectSitesService
    {
        private readonly MfspContext _context;

        public UpdateProjectSitesService(MfspContext context)
        {
            _context = context;
        }

        public async Task Execute(
            string projectId, 
            UpdateProjectSitesRequest request,
            ProjectSiteType siteType)
        {
            var dbProject = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

            if (dbProject == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found");
            }

            var matchingSites = await _context.Property
                .Where(p => p.PRid == dbProject.Rid)
                .ToListAsync();

            Property dbSite = null;
            string dbTypeOfSite = null;

            if (siteType == ProjectSiteType.Permanent)
            {
                dbSite = matchingSites.FirstOrDefault(p => p.IsPermanentSite());
                dbTypeOfSite = "Main";
            }
            else if (siteType == ProjectSiteType.Temporary)
            {
                dbSite = matchingSites.FirstOrDefault(p => p.IsTemporarySite());
                dbTypeOfSite = ProjectSiteType.Temporary.ToString();
            }

            UpdateSite(request.Site, dbSite, dbProject.Rid, dbTypeOfSite);

            await _context.SaveChangesAsync();
        }

        private void UpdateSite(ProjectSite projectSite, Property property, string rid, string tos)
        {
            var dbSite = property;

            if (dbSite == null)
            {
                dbSite = new Property();
                dbSite.PRid = rid;
                dbSite.Rid = IdentifierBuilder.BuildRid();
                dbSite.Tos = tos;
                _context.Property.Add(dbSite);
            }

            dbSite.SiteNameOfSite = projectSite.Address.AddressLine1;
            dbSite.SiteAddressOfSite = projectSite.Address.AddressLine2;
            dbSite.SitePostcodeOfSite = projectSite.Address.Postcode;
        }
    }
}
