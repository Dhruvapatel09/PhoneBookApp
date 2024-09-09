using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiApplicationCore.Migrations
{
    public partial class AlterPhoneBookModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByte",
                table: "phoneBookModels",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageByte",
                table: "phoneBookModels");
        }
    }
}
