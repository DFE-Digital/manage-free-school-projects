using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToKpi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NewSchoolClosingDateForProposals",
                schema: "dbo",
                table: "KPI",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewSchoolDateForConditionsToBeMet",
                schema: "dbo",
                table: "KPI",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewSchoolDateOfDecision",
                schema: "dbo",
                table: "KPI",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewSchoolDecisionMaker",
                schema: "dbo",
                table: "KPI",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewSchoolSpecificationPublicationDate",
                schema: "dbo",
                table: "KPI",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewSchoolClosingDateForProposals",
                schema: "dbo",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "NewSchoolDateForConditionsToBeMet",
                schema: "dbo",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "NewSchoolDateOfDecision",
                schema: "dbo",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "NewSchoolDecisionMaker",
                schema: "dbo",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "NewSchoolSpecificationPublicationDate",
                schema: "dbo",
                table: "KPI");
        }
    }
}
