using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEstablishmentTypeToKpi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "School Details.Establishment Type",
                schema: "dbo",
                table: "KPI",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "School Details.Establishment Type",
                schema: "dbo",
                table: "KPI");
        }
    }
}
