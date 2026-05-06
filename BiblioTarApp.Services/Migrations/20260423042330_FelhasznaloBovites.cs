using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTarApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class FelhasznaloBovites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buntetesek_Felhasznalok_FelhasznaloId",
                table: "Buntetesek");

            migrationBuilder.DropForeignKey(
                name: "FK_Buntetesek_Foglalasok_FoglalasId",
                table: "Buntetesek");

            migrationBuilder.AddColumn<int>(
                name: "Szerepkor",
                table: "Felhasznalok",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Telefonszam",
                table: "Felhasznalok",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Buntetesek_Felhasznalok_FelhasznaloId",
                table: "Buntetesek",
                column: "FelhasznaloId",
                principalTable: "Felhasznalok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buntetesek_Foglalasok_FoglalasId",
                table: "Buntetesek",
                column: "FoglalasId",
                principalTable: "Foglalasok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buntetesek_Felhasznalok_FelhasznaloId",
                table: "Buntetesek");

            migrationBuilder.DropForeignKey(
                name: "FK_Buntetesek_Foglalasok_FoglalasId",
                table: "Buntetesek");

            migrationBuilder.DropColumn(
                name: "Szerepkor",
                table: "Felhasznalok");

            migrationBuilder.DropColumn(
                name: "Telefonszam",
                table: "Felhasznalok");

            migrationBuilder.AddForeignKey(
                name: "FK_Buntetesek_Felhasznalok_FelhasznaloId",
                table: "Buntetesek",
                column: "FelhasznaloId",
                principalTable: "Felhasznalok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Buntetesek_Foglalasok_FoglalasId",
                table: "Buntetesek",
                column: "FoglalasId",
                principalTable: "Foglalasok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
