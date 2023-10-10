﻿using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dfe.ManageFreeSchoolProjects.Data.Configuration.Existing;

public partial class TasksConfiguration : IEntityTypeConfiguration<Tasks>
{
    public void Configure(EntityTypeBuilder<Tasks> builder)
    {
        builder
            .ToTable("Tasks", "dbo")
            .HasKey(tasks => new { tasks.Rid, tasks.TaskName });

        builder.Property(e => e.Rid)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("RID");

        var taskConverter = new EnumToStringConverter<TaskName>();

        builder.Property(e => e.TaskName)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("Task Name")
            .HasConversion(taskConverter);

        var statusConverter = new EnumToStringConverter<Status>();

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("Status")
            .HasConversion(statusConverter);
    }
}