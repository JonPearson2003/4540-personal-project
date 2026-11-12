using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTrack.Api.Data.Migrations;

/// <inheritdoc />
public partial class _20261112191646_FixWeek3ModelDesign : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "RecieptInvoices",
            newName: "ReceiptInvoices");

        migrationBuilder.RenameColumn(
            name: "purchaseDate",
            table: "ReceiptInvoices",
            newName: "PurchaseDate");

        migrationBuilder.DropIndex(
            name: "IX_ProjectMaterials_ProjectId",
            table: "ProjectMaterials");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MaterialUsages",
            table: "MaterialUsages");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MaterialRequests",
            table: "MaterialRequests");

        migrationBuilder.RenameColumn(
            name: "AssignedAt",
            table: "MaterialUsages",
            newName: "UsedAt");

        migrationBuilder.AlterColumn<int>(
            name: "ProjectId",
            table: "TimeRecords",
            type: "INTEGER",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        migrationBuilder.AlterColumn<DateTime>(
            name: "EndTime",
            table: "TimeRecords",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "TEXT");

        migrationBuilder.AlterColumn<TimeSpan>(
            name: "Duration",
            table: "TimeRecords",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(TimeSpan),
            oldType: "TEXT");

        migrationBuilder.AddColumn<int>(
            name: "MaterialUsageId",
            table: "MaterialUsages",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0)
            .Annotation("Sqlite:Autoincrement", true);

        migrationBuilder.AddColumn<int>(
            name: "MaterialRequestId",
            table: "MaterialRequests",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0)
            .Annotation("Sqlite:Autoincrement", true);

        migrationBuilder.AddColumn<int>(
            name: "ReceiptInvoiceId",
            table: "ReceiptInvoices",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0)
            .Annotation("Sqlite:Autoincrement", true);

        migrationBuilder.AddColumn<string>(
            name: "Notes",
            table: "MaterialRequests",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Priority",
            table: "MaterialRequests",
            type: "TEXT",
            maxLength: 20,
            nullable: false,
            defaultValue: "Normal");

        migrationBuilder.AddColumn<int>(
            name: "QuantityRequested",
            table: "MaterialRequests",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<DateTime>(
            name: "ReviewedAt",
            table: "MaterialRequests",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "ReviewedByUserId",
            table: "MaterialRequests",
            type: "INTEGER",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Status",
            table: "MaterialRequests",
            type: "TEXT",
            maxLength: 30,
            nullable: false,
            defaultValue: "Pending");

        migrationBuilder.AddColumn<string>(
            name: "Unit",
            table: "MaterialRequests",
            type: "TEXT",
            maxLength: 40,
            nullable: false,
            defaultValue: "each");

        migrationBuilder.AddUniqueConstraint(
            name: "AK_ProjectMaterials_ProjectId_ProjectMaterialId",
            table: "ProjectMaterials",
            columns: new[] { "ProjectId", "ProjectMaterialId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_MaterialUsages",
            table: "MaterialUsages",
            column: "MaterialUsageId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MaterialRequests",
            table: "MaterialRequests",
            column: "MaterialRequestId");

        migrationBuilder.DropPrimaryKey(
            name: "PK_RecieptInvoices",
            table: "ReceiptInvoices");

        migrationBuilder.AlterColumn<decimal>(
            name: "Amount",
            table: "ReceiptInvoices",
            type: "TEXT",
            precision: 18,
            scale: 2,
            nullable: false,
            oldClrType: typeof(double),
            oldType: "REAL");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ReceiptInvoices",
            table: "ReceiptInvoices",
            column: "ReceiptInvoiceId");

        migrationBuilder.DropIndex(
            name: "IX_RecieptInvoices_UserId",
            table: "ReceiptInvoices");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialUsages_ProjectId",
            table: "MaterialUsages",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_ProjectId",
            table: "MaterialRequests",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialRequests_ReviewedByUserId",
            table: "MaterialRequests",
            column: "ReviewedByUserId");

        migrationBuilder.CreateIndex(
            name: "IX_ReceiptInvoices_ProjectId",
            table: "ReceiptInvoices",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_ReceiptInvoices_UserId",
            table: "ReceiptInvoices",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialRequests_Users_ReviewedByUserId",
            table: "MaterialRequests",
            column: "ReviewedByUserId",
            principalTable: "Users",
            principalColumn: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialRequests_Users_ReviewedByUserId",
            table: "MaterialRequests");

        migrationBuilder.DropUniqueConstraint(
            name: "AK_ProjectMaterials_ProjectId_ProjectMaterialId",
            table: "ProjectMaterials");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MaterialUsages",
            table: "MaterialUsages");

        migrationBuilder.DropIndex(
            name: "IX_MaterialUsages_ProjectId",
            table: "MaterialUsages");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MaterialRequests",
            table: "MaterialRequests");

        migrationBuilder.DropIndex(
            name: "IX_MaterialRequests_ProjectId",
            table: "MaterialRequests");

        migrationBuilder.DropIndex(
            name: "IX_MaterialRequests_ReviewedByUserId",
            table: "MaterialRequests");

        migrationBuilder.DropIndex(
            name: "IX_ReceiptInvoices_ProjectId",
            table: "ReceiptInvoices");

        migrationBuilder.DropIndex(
            name: "IX_ReceiptInvoices_UserId",
            table: "ReceiptInvoices");

        migrationBuilder.DropColumn(
            name: "MaterialUsageId",
            table: "MaterialUsages");

        migrationBuilder.DropColumn(
            name: "MaterialRequestId",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "Notes",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "Priority",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "QuantityRequested",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "ReviewedAt",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "ReviewedByUserId",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "Status",
            table: "MaterialRequests");

        migrationBuilder.DropColumn(
            name: "Unit",
            table: "MaterialRequests");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ReceiptInvoices",
            table: "ReceiptInvoices");

        migrationBuilder.DropColumn(
            name: "ReceiptInvoiceId",
            table: "ReceiptInvoices");

        migrationBuilder.AlterColumn<double>(
            name: "Amount",
            table: "ReceiptInvoices",
            type: "REAL",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "TEXT",
            oldPrecision: 18,
            oldScale: 2);

        migrationBuilder.RenameColumn(
            name: "PurchaseDate",
            table: "ReceiptInvoices",
            newName: "purchaseDate");

        migrationBuilder.RenameTable(
            name: "ReceiptInvoices",
            newName: "RecieptInvoices");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RecieptInvoices",
            table: "RecieptInvoices",
            columns: new[] { "ProjectId", "UserId", "Vendor" });

        migrationBuilder.RenameColumn(
            name: "UsedAt",
            table: "MaterialUsages",
            newName: "AssignedAt");

        migrationBuilder.AlterColumn<int>(
            name: "ProjectId",
            table: "TimeRecords",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "INTEGER",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "EndTime",
            table: "TimeRecords",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AlterColumn<TimeSpan>(
            name: "Duration",
            table: "TimeRecords",
            type: "TEXT",
            nullable: false,
            defaultValue: new TimeSpan(0, 0, 0, 0, 0),
            oldClrType: typeof(TimeSpan),
            oldType: "TEXT",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_MaterialUsages",
            table: "MaterialUsages",
            columns: new[] { "ProjectId", "UserId", "MaterialId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_MaterialRequests",
            table: "MaterialRequests",
            columns: new[] { "ProjectId", "RequestedByUserId", "MaterialId" });

        migrationBuilder.CreateIndex(
            name: "IX_ProjectMaterials_ProjectId",
            table: "ProjectMaterials",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_RecieptInvoices_UserId",
            table: "RecieptInvoices",
            column: "UserId");
    }
}
