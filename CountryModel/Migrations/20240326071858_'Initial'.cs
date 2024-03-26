using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryModel.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Club",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ClubLeague = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ClubCountry = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tmp_ms_x_AF82112AB7EE1730", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PlayerPos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PlayerNationality = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tmp_ms_x_757BC9A0CB74E8F6", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerClub",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerClub", x => new { x.ClubId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_PlayerClub_Club",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId");
                    table.ForeignKey(
                        name: "FK_PlayerClub_Player",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerClub_PlayerId",
                table: "PlayerClub",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerClub");

            migrationBuilder.DropTable(
                name: "Club");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
