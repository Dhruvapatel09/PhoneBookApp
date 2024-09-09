using Microsoft.EntityFrameworkCore.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

#nullable disable

namespace PhoneBookApp.Migrations
{
    public partial class SeedphoneBookModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "phoneBookModels",
                columns: new[] { "PhoneId"
      ,"FirstName"
      ,"LastName"
      ,"Email"
      ,"PhoneNumber"
      ,"Company" },
                values: new object[,]
                {
                    {1,"Dhruva","Patel","abc@gmail.com","1234567890","civica"},
                    {2,"khushi","Patel","pqy@gmail.com","1234576890","civica1"},
                    {3,"abc","xyz","xyz@gmail.com","1235467890","civica2"},


                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "phoneBookModels",
                keyColumn: "PhoneId",
                keyValues: new object[] { 1, 2, 3 });
        }
    }
}
