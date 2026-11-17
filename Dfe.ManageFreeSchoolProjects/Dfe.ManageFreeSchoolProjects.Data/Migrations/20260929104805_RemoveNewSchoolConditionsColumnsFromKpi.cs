using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNewSchoolConditionsColumnsFromKpi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The task status rows outlive the enum member, and GetAllTasksStatusService counts
            // every row it finds when totalling completed tasks while only counting the tasks it
            // still asks for in the denominator. Leaving them behind would report a project as
            // having completed more sections than exist.
            migrationBuilder.Sql("DELETE FROM [mfsp].[Tasks] WHERE [Task Name] = 'NewSchoolConditions';");

            migrationBuilder.DropColumn(
                name: "NewSchoolConditions",
                schema: "dbo",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "NewSchoolConditionsDescription",
                schema: "dbo",
                table: "KPI");
        }

        /// <inheritdoc />
        /// <remarks>
        /// Down restores the columns empty. KPI is system-versioned, so Up drops these columns from
        /// KPIHistory as well and the stored conditions are not recoverable from history. The
        /// deleted task status rows do survive in the Tasks history table.
        /// </remarks>
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
