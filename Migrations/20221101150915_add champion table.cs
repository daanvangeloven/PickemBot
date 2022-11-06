using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickemBot.Migrations
{
    public partial class addchampiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChampionPicksID",
                table: "Pickers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChampionPicks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WinnerID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionPicks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChampionPicks_Teams_WinnerID",
                        column: x => x.WinnerID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChampionPicksTeam",
                columns: table => new
                {
                    PickedQuarterFinalsID = table.Column<int>(type: "INTEGER", nullable: false),
                    QuarterFinalistsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionPicksTeam", x => new { x.PickedQuarterFinalsID, x.QuarterFinalistsID });
                    table.ForeignKey(
                        name: "FK_ChampionPicksTeam_ChampionPicks_PickedQuarterFinalsID",
                        column: x => x.PickedQuarterFinalsID,
                        principalTable: "ChampionPicks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChampionPicksTeam_Teams_QuarterFinalistsID",
                        column: x => x.QuarterFinalistsID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChampionPicksTeam1",
                columns: table => new
                {
                    PickedSemiFinalsID = table.Column<int>(type: "INTEGER", nullable: false),
                    SemiFinalistsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionPicksTeam1", x => new { x.PickedSemiFinalsID, x.SemiFinalistsID });
                    table.ForeignKey(
                        name: "FK_ChampionPicksTeam1_ChampionPicks_PickedSemiFinalsID",
                        column: x => x.PickedSemiFinalsID,
                        principalTable: "ChampionPicks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChampionPicksTeam1_Teams_SemiFinalistsID",
                        column: x => x.SemiFinalistsID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pickers_ChampionPicksID",
                table: "Pickers",
                column: "ChampionPicksID");

            migrationBuilder.CreateIndex(
                name: "IX_ChampionPicks_WinnerID",
                table: "ChampionPicks",
                column: "WinnerID");

            migrationBuilder.CreateIndex(
                name: "IX_ChampionPicksTeam_QuarterFinalistsID",
                table: "ChampionPicksTeam",
                column: "QuarterFinalistsID");

            migrationBuilder.CreateIndex(
                name: "IX_ChampionPicksTeam1_SemiFinalistsID",
                table: "ChampionPicksTeam1",
                column: "SemiFinalistsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pickers_ChampionPicks_ChampionPicksID",
                table: "Pickers",
                column: "ChampionPicksID",
                principalTable: "ChampionPicks",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pickers_ChampionPicks_ChampionPicksID",
                table: "Pickers");

            migrationBuilder.DropTable(
                name: "ChampionPicksTeam");

            migrationBuilder.DropTable(
                name: "ChampionPicksTeam1");

            migrationBuilder.DropTable(
                name: "ChampionPicks");

            migrationBuilder.DropIndex(
                name: "IX_Pickers_ChampionPicksID",
                table: "Pickers");

            migrationBuilder.DropColumn(
                name: "ChampionPicksID",
                table: "Pickers");
        }
    }
}
