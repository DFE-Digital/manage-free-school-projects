using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.ManageFreeSchoolProjects.Data.Configuration.Existing
{
    public partial class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.HasKey(e => e.Rid);

            builder.ToTable("Proposal", "dbo", e => e.IsTemporal());

            builder.Property(e => e.Rid)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("RID");
            builder.Property(e => e.ProjectId)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ProjectId");
            builder.Property(e => e.Proposer)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.TrustReferenceNumber)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("TrustReferenceNumber");
            builder.Property(e => e.TrustName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TrustName");
            builder.Property(e => e.TrustType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TrustType");

            builder.Property(e => e.NameOfDiocese)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NameOfDiocese");
            builder.Property(e => e.FaithOfDiocese)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FaithOfDiocese");

            builder.Property(e => e.NameOfOtherReligiousOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NameOfOtherReligiousOrganisation");
            builder.Property(e => e.FaithTypeOfOtherReligiousOrganisation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FaithTypeOfOtherReligiousOrganisation");
            builder.Property(e => e.OtherFaithTypeOfOtherReligiousOrganisation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("OtherFaithTypeOfOtherReligiousOrganisation");

            builder.Property(e => e.OtherLocalAuthority)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("OtherLocalAuthority");

            builder.Property(e => e.JointProposalLocalAuthority)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("JointProposalLocalAuthority");

            builder.Property(e => e.ProposedFaithStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProposedFaithStatus");
            builder.Property(e => e.ProposedFaithType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProposedFaithType");
            builder.Property(e => e.OtherFaithType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("OtherFaithType");

            AuditConfiguration.Apply(builder);
        }
    }
}
