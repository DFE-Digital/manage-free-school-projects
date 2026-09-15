using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(MfspContext))]
    [Migration("20260821134200_AddKpiFsgLeadContactEmailIndex")]
    public partial class AddKpiFsgLeadContactEmailIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KPI_KeyContactsFsgLeadContactEmail",
                schema: "dbo",
                table: "KPI",
                column: "Key Contacts.FSG lead contact email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KPI_KeyContactsFsgLeadContactEmail",
                schema: "dbo",
                table: "KPI");
        }
    }
}
