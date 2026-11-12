using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTrack.Api.Data.Migrations;

/// <inheritdoc />
public partial class _20261112191824_AddModelForeignKeys : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            UPDATE "TimeRecords"
            SET "ProjectId" = NULL
            WHERE "ProjectId" IS NOT NULL
              AND NOT EXISTS (
                  SELECT 1
                  FROM "Projects"
                  WHERE "Projects"."ProjectId" = "TimeRecords"."ProjectId");
            """);

        migrationBuilder.DropForeignKey(
            name: "FK_MaterialRequests_ProjectMaterials_MaterialId",
            table: "MaterialRequests");

        migrationBuilder.DropForeignKey(
            name: "FK_MaterialUsages_ProjectMaterials_MaterialId",
            table: "MaterialUsages");

        migrationBuilder.DropIndex(
            name: "IX_MaterialUsages_MaterialId",
            table: "MaterialUsages");

        migrationBuilder.DropIndex(
            name: "IX_MaterialUsages_ProjectId",
            table: "MaterialUsages");

        migrationBuilder.DropIndex(
            name: "IX_MaterialRequests_MaterialId",
            table: "MaterialRequests");

        migrationBuilder.DropIndex(
            name: "IX_MaterialRequests_ProjectId",
            table: "MaterialRequests");

        migrationBuilder.CreateIndex(
            name: "IX_TimeRecords_ProjectId",
            table: "TimeRecords",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialUsages_ProjectId_MaterialId",
            table: "MaterialUsages",
            columns: new[] { "ProjectId", "MaterialId" });

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_ProjectId_MaterialId",
            table: "MaterialRequests",
            columns: new[] { "ProjectId", "MaterialId" });

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialRequests_ProjectMaterials_ProjectId_MaterialId",
            table: "MaterialRequests",
            columns: new[] { "ProjectId", "MaterialId" },
            principalTable: "ProjectMaterials",
            principalColumns: new[] { "ProjectId", "ProjectMaterialId" },
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialUsages_ProjectMaterials_ProjectId_MaterialId",
            table: "MaterialUsages",
            columns: new[] { "ProjectId", "MaterialId" },
            principalTable: "ProjectMaterials",
            principalColumns: new[] { "ProjectId", "ProjectMaterialId" },
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_TimeRecords_Projects_ProjectId",
            table: "TimeRecords",
            column: "ProjectId",
            principalTable: "Projects",
            principalColumn: "ProjectId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialRequests_ProjectMaterials_ProjectId_MaterialId",
            table: "MaterialRequests");

        migrationBuilder.DropForeignKey(
            name: "FK_MaterialUsages_ProjectMaterials_ProjectId_MaterialId",
            table: "MaterialUsages");

        migrationBuilder.DropForeignKey(
            name: "FK_TimeRecords_Projects_ProjectId",
            table: "TimeRecords");

        migrationBuilder.DropIndex(
            name: "IX_TimeRecords_ProjectId",
            table: "TimeRecords");

        migrationBuilder.DropIndex(
            name: "IX_MaterialUsages_ProjectId_MaterialId",
            table: "MaterialUsages");

        migrationBuilder.DropIndex(
            name: "IX_MaterialRequests_ProjectId_MaterialId",
            table: "MaterialRequests");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialUsages_MaterialId",
            table: "MaterialUsages",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialUsages_ProjectId",
            table: "MaterialUsages",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_MaterialId",
            table: "MaterialRequests",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_ProjectId",
            table: "MaterialRequests",
            column: "ProjectId");

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialRequests_ProjectMaterials_MaterialId",
            table: "MaterialRequests",
            column: "MaterialId",
            principalTable: "ProjectMaterials",
            principalColumn: "ProjectMaterialId",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialUsages_ProjectMaterials_MaterialId",
            table: "MaterialUsages",
            column: "MaterialId",
            principalTable: "ProjectMaterials",
            principalColumn: "ProjectMaterialId",
            onDelete: ReferentialAction.Cascade);
    }
}
