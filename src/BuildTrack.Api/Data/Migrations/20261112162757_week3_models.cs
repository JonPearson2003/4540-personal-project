using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTrack.Api.Data.Migrations;

/// <inheritdoc />
public partial class _20261112162757_week3_models : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ProjectId",
            table: "TimeRecords",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "MaterialRequests",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                RequestedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialRequests", x => new { x.ProjectId, x.RequestedByUserId, x.MaterialId });
                table.ForeignKey(
                    name: "FK_MaterialRequests_ProjectMaterials_MaterialId",
                    column: x => x.MaterialId,
                    principalTable: "ProjectMaterials",
                    principalColumn: "ProjectMaterialId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialRequests_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialRequests_Users_RequestedByUserId",
                    column: x => x.RequestedByUserId,
                    principalTable: "Users",
                    principalColumn: "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MaterialUsages",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                QuantityUsed = table.Column<int>(type: "INTEGER", nullable: false),
                AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialUsages", x => new { x.ProjectId, x.UserId, x.MaterialId });
                table.ForeignKey(
                    name: "FK_MaterialUsages_ProjectMaterials_MaterialId",
                    column: x => x.MaterialId,
                    principalTable: "ProjectMaterials",
                    principalColumn: "ProjectMaterialId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialUsages_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialUsages_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ProjectAssignments",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectAssignments", x => new { x.ProjectId, x.UserId });
                table.ForeignKey(
                    name: "FK_ProjectAssignments_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProjectAssignments_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RecieptInvoices",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                Vendor = table.Column<string>(type: "TEXT", nullable: false),
                Amount = table.Column<double>(type: "REAL", nullable: false),
                purchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                InvoiceImage = table.Column<byte[]>(type: "BLOB", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecieptInvoices", x => new { x.ProjectId, x.UserId, x.Vendor });
                table.ForeignKey(
                    name: "FK_RecieptInvoices_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RecieptInvoices_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_MaterialId",
            table: "MaterialRequests",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_RequestedByUserId",
            table: "MaterialRequests",
            column: "RequestedByUserId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialUsages_MaterialId",
            table: "MaterialUsages",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialUsages_UserId",
            table: "MaterialUsages",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectAssignments_UserId",
            table: "ProjectAssignments",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_RecieptInvoices_UserId",
            table: "RecieptInvoices",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MaterialRequests");

        migrationBuilder.DropTable(
            name: "MaterialUsages");

        migrationBuilder.DropTable(
            name: "ProjectAssignments");

        migrationBuilder.DropTable(
            name: "RecieptInvoices");

        migrationBuilder.DropColumn(
            name: "ProjectId",
            table: "TimeRecords");
    }
}
