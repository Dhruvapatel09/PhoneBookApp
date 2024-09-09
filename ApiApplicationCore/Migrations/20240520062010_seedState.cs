using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiApplicationCore.Migrations
{
    public partial class seedState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "States",
            columns: new[] { "StateId", "StateName", "CountryId" },
            values: new object[,]
            {
                    {1,"Gujarat",1 },
                    {2,"Delhi",1 },
                    {3,"Maharastra",1 },
                    {4,"Hariyana",1 },
                    {5,"Rajasthan",1 },
                    {6,"Sydney",2 },
                    {7,"Malbourn",2 },
                    {8,"London",3 },
                    {9,"Edinburgh",3 },
                    {10,"Manchester",3 },
                    {11,"Calfornia",4 },
                    {12,"Ohio",4 },
                    {13,"New Jersey",4 },
                    {14,"Ontario",5 },
                    {15,"Oshawa",5 },
                    {16,"Hamilton",5 },
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "OrderId",
            keyValue: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }
        );

        }
    }
}
