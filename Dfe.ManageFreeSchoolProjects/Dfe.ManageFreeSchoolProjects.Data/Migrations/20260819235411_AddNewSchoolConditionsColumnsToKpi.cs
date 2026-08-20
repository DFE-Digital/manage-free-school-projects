using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSchoolConditionsColumnsToKpi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewSchoolConditions",
                schema: "dbo",
                table: "KPI",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewSchoolConditionsDescription",
                schema: "dbo",
                table: "KPI",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewSchoolConditions",
                schema: "dbo",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "NewSchoolConditionsDescription",
                schema: "dbo",
                table: "KPI");
        }
    }
}
