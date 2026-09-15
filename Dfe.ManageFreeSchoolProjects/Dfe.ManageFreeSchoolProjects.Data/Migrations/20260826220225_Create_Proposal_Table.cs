using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageFreeSchoolProjects.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_Proposal_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proposal",
                schema: "dbo",
                columns: table => new
                {
                    RID = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    ProjectId = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    Proposer = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    TrustReferenceNumber = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: true),
                    TrustName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TrustType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NameOfDiocese = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FaithOfDiocese = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NameOfOtherReligiousOrganisation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FaithTypeOfOtherReligiousOrganisation = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OtherFaithTypeOfOtherReligiousOrganisation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    OtherLocalAuthority = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    JointProposalLocalAuthority = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ProposedFaithStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProposedFaithType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OtherFaithType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposal", x => x.RID);
                    table.ForeignKey(
                        name: "FK_Proposal_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "mfsp",
                        principalTable: "User",
                        principalColumn: "Id");
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ProposalHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "dbo")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_UpdatedByUserId",
                schema: "dbo",
                table: "Proposal",
                column: "UpdatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proposal",
                schema: "dbo")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ProposalHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "dbo")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
