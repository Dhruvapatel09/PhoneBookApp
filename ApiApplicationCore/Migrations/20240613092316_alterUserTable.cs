using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiApplicationCore.Migrations
{
    public partial class alterUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "users");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "users");
        }
    }
}
