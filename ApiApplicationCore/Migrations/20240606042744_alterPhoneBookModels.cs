using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiApplicationCore.Migrations
{
    public partial class alterPhoneBookModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "phoneBookModels",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "phoneBookModels");
        }
    }
}
