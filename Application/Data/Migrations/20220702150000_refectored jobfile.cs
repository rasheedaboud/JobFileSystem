using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Data.Migrations
{
    public partial class refectoredjobfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobFiles_Contacts_ClientId",
                table: "JobFiles");

            migrationBuilder.DropIndex(
                name: "IX_JobFiles_ClientId",
                table: "JobFiles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "JobFiles",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "JobFiles",
                newName: "EstimateId");

            migrationBuilder.RenameColumn(
                name: "LoggedOn",
                table: "Estimates",
                newName: "RequestForQuoteReceived");

            migrationBuilder.AddColumn<string>(
                name: "ContactId",
                table: "JobFiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "JobFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderNumber",
                table: "JobFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Estimates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_JobFiles_ContactId",
                table: "JobFiles",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_JobFiles_EstimateId",
                table: "JobFiles",
                column: "EstimateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobFiles_Contacts_ContactId",
                table: "JobFiles",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobFiles_Estimates_EstimateId",
                table: "JobFiles",
                column: "EstimateId",
                principalTable: "Estimates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobFiles_Contacts_ContactId",
                table: "JobFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobFiles_Estimates_EstimateId",
                table: "JobFiles");

            migrationBuilder.DropIndex(
                name: "IX_JobFiles_ContactId",
                table: "JobFiles");

            migrationBuilder.DropIndex(
                name: "IX_JobFiles_EstimateId",
                table: "JobFiles");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "JobFiles");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "JobFiles");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderNumber",
                table: "JobFiles");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Estimates");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "JobFiles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EstimateId",
                table: "JobFiles",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "RequestForQuoteReceived",
                table: "Estimates",
                newName: "LoggedOn");

            migrationBuilder.CreateIndex(
                name: "IX_JobFiles_ClientId",
                table: "JobFiles",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobFiles_Contacts_ClientId",
                table: "JobFiles",
                column: "ClientId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
