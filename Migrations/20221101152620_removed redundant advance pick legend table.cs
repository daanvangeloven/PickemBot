using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickemBot.Migrations
{
    public partial class removedredundantadvancepicklegendtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pickers_ChallengerPicks_ChallengerPicksID",
                table: "Pickers");

            migrationBuilder.DropForeignKey(
                name: "FK_Pickers_LegendPicks_LegendPicksID",
                table: "Pickers");

            migrationBuilder.DropTable(
                name: "ChallengerPicksTeam");

            migrationBuilder.DropTable(
                name: "LegendPicksTeam");

            migrationBuilder.DropTable(
                name: "ChallengerPicks");

            migrationBuilder.DropTable(
                name: "LegendPicks");

            migrationBuilder.CreateTable(
                name: "AdvancePicks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UndefeatedID = table.Column<int>(type: "INTEGER", nullable: true),
                    NoWinID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancePicks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdvancePicks_Teams_NoWinID",
                        column: x => x.NoWinID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AdvancePicks_Teams_UndefeatedID",
                        column: x => x.UndefeatedID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AdvancePicksTeam",
                columns: table => new
                {
                    AdvanceTeamsID = table.Column<int>(type: "INTEGER", nullable: false),
                    PickedByAdvanceID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancePicksTeam", x => new { x.AdvanceTeamsID, x.PickedByAdvanceID });
                    table.ForeignKey(
                        name: "FK_AdvancePicksTeam_AdvancePicks_PickedByAdvanceID",
                        column: x => x.PickedByAdvanceID,
                        principalTable: "AdvancePicks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvancePicksTeam_Teams_AdvanceTeamsID",
                        column: x => x.AdvanceTeamsID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePicks_NoWinID",
                table: "AdvancePicks",
                column: "NoWinID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePicks_UndefeatedID",
                table: "AdvancePicks",
                column: "UndefeatedID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePicksTeam_PickedByAdvanceID",
                table: "AdvancePicksTeam",
                column: "PickedByAdvanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pickers_AdvancePicks_ChallengerPicksID",
                table: "Pickers",
                column: "ChallengerPicksID",
                principalTable: "AdvancePicks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pickers_AdvancePicks_LegendPicksID",
                table: "Pickers",
                column: "LegendPicksID",
                principalTable: "AdvancePicks",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pickers_AdvancePicks_ChallengerPicksID",
                table: "Pickers");

            migrationBuilder.DropForeignKey(
                name: "FK_Pickers_AdvancePicks_LegendPicksID",
                table: "Pickers");

            migrationBuilder.DropTable(
                name: "AdvancePicksTeam");

            migrationBuilder.DropTable(
                name: "AdvancePicks");

            migrationBuilder.CreateTable(
                name: "ChallengerPicks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoWinID = table.Column<int>(type: "INTEGER", nullable: true),
                    UndefeatedID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengerPicks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChallengerPicks_Teams_NoWinID",
                        column: x => x.NoWinID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ChallengerPicks_Teams_UndefeatedID",
                        column: x => x.UndefeatedID,
                        principalTable: "Teams",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LegendPicks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoWinID = table.Column<int>(type: "INTEGER", nullable: true),
                    UndefeatedID = table.Column<int>(type: "INTEGER", nullable: true)
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
                name: "ChallengerPicksTeam",
                columns: table => new
                {
                    AdvanceTeamsID = table.Column<int>(type: "INTEGER", nullable: false),
                    PickedByChallengerID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengerPicksTeam", x => new { x.AdvanceTeamsID, x.PickedByChallengerID });
                    table.ForeignKey(
                        name: "FK_ChallengerPicksTeam_ChallengerPicks_PickedByChallengerID",
                        column: x => x.PickedByChallengerID,
                        principalTable: "ChallengerPicks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengerPicksTeam_Teams_AdvanceTeamsID",
                        column: x => x.AdvanceTeamsID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_ChallengerPicks_NoWinID",
                table: "ChallengerPicks",
                column: "NoWinID");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengerPicks_UndefeatedID",
                table: "ChallengerPicks",
                column: "UndefeatedID");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengerPicksTeam_PickedByChallengerID",
                table: "ChallengerPicksTeam",
                column: "PickedByChallengerID");

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
                name: "FK_Pickers_ChallengerPicks_ChallengerPicksID",
                table: "Pickers",
                column: "ChallengerPicksID",
                principalTable: "ChallengerPicks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pickers_LegendPicks_LegendPicksID",
                table: "Pickers",
                column: "LegendPicksID",
                principalTable: "LegendPicks",
                principalColumn: "ID");
        }
    }
}
