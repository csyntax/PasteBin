using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PasteBin.Data.Migrations
{
    public partial class DeletablePasteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Pastes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pastes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Pastes_IsDeleted",
                table: "Pastes",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pastes_IsDeleted",
                table: "Pastes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Pastes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pastes");
        }
    }
}
