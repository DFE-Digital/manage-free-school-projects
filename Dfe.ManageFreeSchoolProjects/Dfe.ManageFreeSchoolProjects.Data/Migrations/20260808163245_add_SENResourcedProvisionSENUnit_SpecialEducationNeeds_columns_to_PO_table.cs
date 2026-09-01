using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_SENResourcedProvisionSENUnit_SpecialEducationNeeds_columns_to_PO_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PupilNumbersAndCapacityAPResourcesProvision",
                schema: "dbo",
                table: "PO",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PupilNumbersAndCapacitySENResourcedProvisionSENUnit",
                schema: "dbo",
                table: "PO",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PupilNumbersAndCapacityAPResourcesProvision",
                schema: "dbo",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "PupilNumbersAndCapacitySENResourcedProvisionSENUnit",
                schema: "dbo",
                table: "PO");
        }
    }
}
