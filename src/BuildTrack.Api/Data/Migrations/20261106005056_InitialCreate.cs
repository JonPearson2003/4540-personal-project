using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTrack.Api.Data.Migrations;

/// <inheritdoc />
public partial class _20261106005056_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Projects",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                Location = table.Column<string>(type: "TEXT", nullable: false),
                StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.ProjectId);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                UserId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                Password = table.Column<string>(type: "TEXT", nullable: false),
                Role = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.UserId);
            });

        migrationBuilder.CreateTable(
            name: "ProjectMaterials",
            columns: table => new
            {
                ProjectMaterialId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                Price = table.Column<decimal>(type: "TEXT", nullable: false),
                Quantity = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectMaterials", x => x.ProjectMaterialId);
                table.ForeignKey(
                    name: "FK_ProjectMaterials_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ProjectStatuses",
            columns: table => new
            {
                ProjectStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                ProgressPhoto = table.Column<byte[]>(type: "BLOB", nullable: false),
                Status = table.Column<string>(type: "TEXT", nullable: false),
                Date = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProjectStatuses", x => x.ProjectStatusId);
                table.ForeignKey(
                    name: "FK_ProjectStatuses_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "ProjectId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TimeRecords",
            columns: table => new
            {
                TimeRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TimeRecords", x => x.TimeRecordId);
                table.ForeignKey(
                    name: "FK_TimeRecords_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProjectMaterials_ProjectId",
            table: "ProjectMaterials",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_ProjectStatuses_ProjectId",
            table: "ProjectStatuses",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_TimeRecords_UserId",
            table: "TimeRecords",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProjectMaterials");

        migrationBuilder.DropTable(
            name: "ProjectStatuses");

        migrationBuilder.DropTable(
            name: "TimeRecords");

        migrationBuilder.DropTable(
            name: "Projects");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
