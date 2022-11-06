using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickemBot.Migrations
{
    public partial class please : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    EmoteName = table.Column<string>(type: "TEXT", nullable: false),
                    Colour = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChallengerPicks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UndefeatedID = table.Column<int>(type: "INTEGER", nullable: true),
                    NoWinID = table.Column<int>(type: "INTEGER", nullable: true)
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
                name: "ChallengerPicksTeam",
                columns: table => new
                {
                    AdvanceTeamsID = table.Column<int>(type: "INTEGER", nullable: false),
                    PickedByID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengerPicksTeam", x => new { x.AdvanceTeamsID, x.PickedByID });
                    table.ForeignKey(
                        name: "FK_ChallengerPicksTeam_ChallengerPicks_PickedByID",
                        column: x => x.PickedByID,
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
                name: "Pickers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DiscordID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ChallengerPicksID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pickers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pickers_ChallengerPicks_ChallengerPicksID",
                        column: x => x.ChallengerPicksID,
                        principalTable: "ChallengerPicks",
                        principalColumn: "ID");
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
                name: "IX_ChallengerPicksTeam_PickedByID",
                table: "ChallengerPicksTeam",
                column: "PickedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Pickers_ChallengerPicksID",
                table: "Pickers",
                column: "ChallengerPicksID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengerPicksTeam");

            migrationBuilder.DropTable(
                name: "Pickers");

            migrationBuilder.DropTable(
                name: "ChallengerPicks");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
