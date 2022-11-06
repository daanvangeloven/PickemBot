using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickemBot.Migrations
{
    public partial class addedlegendpicktopicker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LegendPicksID",
                table: "Pickers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pickers_LegendPicksID",
                table: "Pickers",
                column: "LegendPicksID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pickers_LegendPicks_LegendPicksID",
                table: "Pickers",
                column: "LegendPicksID",
                principalTable: "LegendPicks",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pickers_LegendPicks_LegendPicksID",
                table: "Pickers");

            migrationBuilder.DropIndex(
                name: "IX_Pickers_LegendPicksID",
                table: "Pickers");

            migrationBuilder.DropColumn(
                name: "LegendPicksID",
                table: "Pickers");
        }
    }
}
