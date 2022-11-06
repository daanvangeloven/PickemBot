using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickemBot.Migrations
{
    public partial class addedlegendpickdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengerPicksTeam_ChallengerPicks_PickedByID",
                table: "ChallengerPicksTeam");

            migrationBuilder.RenameColumn(
                name: "PickedByID",
                table: "ChallengerPicksTeam",
                newName: "PickedByChallengerID");

            migrationBuilder.RenameIndex(
                name: "IX_ChallengerPicksTeam_PickedByID",
                table: "ChallengerPicksTeam",
                newName: "IX_ChallengerPicksTeam_PickedByChallengerID");

            migrationBuilder.CreateTable(
                name: "LegendPicks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UndefeatedID = table.Column<int>(type: "INTEGER", nullable: true),
                    NoWinID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegendPicks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LegendPicks_Teams_NoWinID",
                        column: x => x.NoWinID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LegendPicks_Teams_UndefeatedID",
                        column: x => x.UndefeatedID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LegendPicksTeam",
                columns: table => new
                {
                    AdvanceTeamsID = table.Column<int>(type: "INTEGER", nullable: false),
                    PickedByLegendID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegendPicksTeam", x => new { x.AdvanceTeamsID, x.PickedByLegendID });
                    table.ForeignKey(
                        name: "FK_LegendPicksTeam_LegendPicks_PickedByLegendID",
                        column: x => x.PickedByLegendID,
                        principalTable: "LegendPicks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegendPicksTeam_Teams_AdvanceTeamsID",
                        column: x => x.AdvanceTeamsID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegendPicks_NoWinID",
                table: "LegendPicks",
                column: "NoWinID");

            migrationBuilder.CreateIndex(
                name: "IX_LegendPicks_UndefeatedID",
                table: "LegendPicks",
                column: "UndefeatedID");

            migrationBuilder.CreateIndex(
                name: "IX_LegendPicksTeam_PickedByLegendID",
                table: "LegendPicksTeam",
                column: "PickedByLegendID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengerPicksTeam_ChallengerPicks_PickedByChallengerID",
                table: "ChallengerPicksTeam",
                column: "PickedByChallengerID",
                principalTable: "ChallengerPicks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengerPicksTeam_ChallengerPicks_PickedByChallengerID",
                table: "ChallengerPicksTeam");

            migrationBuilder.DropTable(
                name: "LegendPicksTeam");

            migrationBuilder.DropTable(
                name: "LegendPicks");

            migrationBuilder.RenameColumn(
                name: "PickedByChallengerID",
                table: "ChallengerPicksTeam",
                newName: "PickedByID");

            migrationBuilder.RenameIndex(
                name: "IX_ChallengerPicksTeam_PickedByChallengerID",
                table: "ChallengerPicksTeam",
                newName: "IX_ChallengerPicksTeam_PickedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengerPicksTeam_ChallengerPicks_PickedByID",
                table: "ChallengerPicksTeam",
                column: "PickedByID",
                principalTable: "ChallengerPicks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
