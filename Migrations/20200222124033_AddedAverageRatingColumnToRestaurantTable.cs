using Microsoft.EntityFrameworkCore.Migrations;

namespace Knock.API.Migrations
{
    public partial class AddedAverageRatingColumnToRestaurantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Restaurants",
                type: "decimal(2,2)",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Restaurants");
        }
    }
}
