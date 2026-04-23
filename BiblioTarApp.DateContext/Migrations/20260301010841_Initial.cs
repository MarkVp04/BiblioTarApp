using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTarApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Felhasznalok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jelszo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Felhasznalok", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Konyvek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Statusz = table.Column<int>(type: "int", nullable: false),
                    Cim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Szerzo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Allapot = table.Column<int>(type: "int", nullable: false),
                    Ertelekes = table.Column<int>(type: "int", nullable: false),
                    Kategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublikalasIdeje = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konyvek", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lakimcek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Iranyitoszam = table.Column<int>(type: "int", nullable: false),
                    Varos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hazszam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FelhasznaloId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lakimcek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lakimcek_Felhasznalok_FelhasznaloId",
                        column: x => x.FelhasznaloId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Foglalasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FelhasznaloId = table.Column<int>(type: "int", nullable: false),
                    KonyvId = table.Column<int>(type: "int", nullable: false),
                    FoglalasIdeje = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hatarido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MeghosszabbitasiLehetosegek = table.Column<int>(type: "int", nullable: false),
                    Statusz = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foglalasok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foglalasok_Felhasznalok_FelhasznaloId",
                        column: x => x.FelhasznaloId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Foglalasok_Konyvek_KonyvId",
                        column: x => x.KonyvId,
                        principalTable: "Konyvek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kolcsonzesek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FelhasznaloId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    KolcsozesIdeje = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statusz = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kolcsonzesek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kolcsonzesek_Felhasznalok_FelhasznaloId",
                        column: x => x.FelhasznaloId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kolcsonzesek_Konyvek_BookId",
                        column: x => x.BookId,
                        principalTable: "Konyvek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Buntetesek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FelhasznaloId = table.Column<int>(type: "int", nullable: false),
                    FoglalasId = table.Column<int>(type: "int", nullable: false),
                    Ar = table.Column<int>(type: "int", nullable: false),
                    FizetesiStatusz = table.Column<bool>(type: "bit", nullable: false),
                    BuntetesIdeje = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buntetesek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buntetesek_Felhasznalok_FelhasznaloId",
                        column: x => x.FelhasznaloId,
                        principalTable: "Felhasznalok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buntetesek_Foglalasok_FoglalasId",
                        column: x => x.FoglalasId,
                        principalTable: "Foglalasok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buntetesek_FelhasznaloId",
                table: "Buntetesek",
                column: "FelhasznaloId");

            migrationBuilder.CreateIndex(
                name: "IX_Buntetesek_FoglalasId",
                table: "Buntetesek",
                column: "FoglalasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foglalasok_FelhasznaloId",
                table: "Foglalasok",
                column: "FelhasznaloId");

            migrationBuilder.CreateIndex(
                name: "IX_Foglalasok_KonyvId",
                table: "Foglalasok",
                column: "KonyvId");

            migrationBuilder.CreateIndex(
                name: "IX_Kolcsonzesek_BookId",
                table: "Kolcsonzesek",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Kolcsonzesek_FelhasznaloId",
                table: "Kolcsonzesek",
                column: "FelhasznaloId");

            migrationBuilder.CreateIndex(
                name: "IX_Lakimcek_FelhasznaloId",
                table: "Lakimcek",
                column: "FelhasznaloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buntetesek");

            migrationBuilder.DropTable(
                name: "Kolcsonzesek");

            migrationBuilder.DropTable(
                name: "Lakimcek");

            migrationBuilder.DropTable(
                name: "Foglalasok");

            migrationBuilder.DropTable(
                name: "Felhasznalok");

            migrationBuilder.DropTable(
                name: "Konyvek");
        }
    }
}
