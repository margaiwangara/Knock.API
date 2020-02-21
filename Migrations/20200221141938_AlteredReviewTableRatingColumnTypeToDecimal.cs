using Microsoft.EntityFrameworkCore.Migrations;

namespace Knock.API.Migrations
{
    public partial class AlteredReviewTableRatingColumnTypeToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Rating",
                table: "Reviews",
                type: "decimal(2,2)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Rating",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "decimal(2,2)");
        }
    }
}
