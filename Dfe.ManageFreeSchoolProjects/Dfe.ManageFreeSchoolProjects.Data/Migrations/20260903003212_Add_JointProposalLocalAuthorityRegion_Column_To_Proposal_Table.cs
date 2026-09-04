using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_JointProposalLocalAuthorityRegion_Column_To_Proposal_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JointProposalLocalAuthorityRegion",
                schema: "dbo",
                table: "Proposal",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JointProposalLocalAuthorityRegion",
                schema: "dbo",
                table: "Proposal");
        }
    }
}
