using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickemBot.Migrations
{
    public partial class addedpingboolstopicker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LegendResult",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChallengeResult",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChallengerPinged",
                table: "Pickers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ChampionPinged",
                table: "Pickers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LegendPinged",
                table: "Pickers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallengerPinged",
                table: "Pickers");

            migrationBuilder.DropColumn(
                name: "ChampionPinged",
                table: "Pickers");

            migrationBuilder.DropColumn(
                name: "LegendPinged",
                table: "Pickers");

            migrationBuilder.AlterColumn<int>(
                name: "LegendResult",
                table: "Teams",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ChallengeResult",
                table: "Teams",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
