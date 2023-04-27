using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Data.Migrations
{
    public partial class addedmaterialtestreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialTestReports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialForm = table.Column<ushort>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Diameter = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Thickness = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTestReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTestReports_Attachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaterialTestReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtention = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTestReports_Attachments", x => new { x.MaterialTestReportId, x.Id });
                    table.ForeignKey(
                        name: "FK_MaterialTestReports_Attachments_MaterialTestReports_MaterialTestReportId",
                        column: x => x.MaterialTestReportId,
                        principalTable: "MaterialTestReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialTestReports_Attachments");

            migrationBuilder.DropTable(
                name: "MaterialTestReports");
        }
    }
}
