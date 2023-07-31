﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dfe.ManageFreeSchoolProjects.Data.Models.Projects;

namespace Dfe.ManageFreeSchoolProjects.Data.Configurations.Projects
{
	public class ProjectConfiguration : IEntityTypeConfiguration<Project>
	{
		public void Configure(EntityTypeBuilder<Project> builder)
		{
			builder.ToTable("Projects", "openFreeSchool");

			builder.HasKey(e => e.Id);

			//builder.HasIndex(x => new { x.CaseUrn, x.CreatedAt }).IsUnique();
		}
	}
}
