using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTarApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lakimcek_Felhasznalok_FelhasznaloId",
                table: "Lakimcek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lakimcek",
                table: "Lakimcek");

            migrationBuilder.DropIndex(
                name: "IX_Lakimcek_FelhasznaloId",
                table: "Lakimcek");

            migrationBuilder.RenameTable(
                name: "Lakimcek",
                newName: "Lakcimek");

            migrationBuilder.AlterColumn<string>(
                name: "Statusz",
                table: "Konyvek",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublikalasIdeje",
                table: "Konyvek",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Kategoria",
                table: "Konyvek",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Ertelekes",
                table: "Konyvek",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Allapot",
                table: "Konyvek",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Isbn",
                table: "Konyvek",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Kiadasev",
                table: "Konyvek",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoglalasId",
                table: "Kolcsonzesek",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FelhasznaloId",
                table: "Lakcimek",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lakcimek",
                table: "Lakcimek",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Kolcsonzesek_FoglalasId",
                table: "Kolcsonzesek",
                column: "FoglalasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lakcimek_FelhasznaloId",
                table: "Lakcimek",
                column: "FelhasznaloId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Kolcsonzesek_Foglalasok_FoglalasId",
                table: "Kolcsonzesek",
                column: "FoglalasId",
                principalTable: "Foglalasok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lakcimek_Felhasznalok_FelhasznaloId",
                table: "Lakcimek",
                column: "FelhasznaloId",
                principalTable: "Felhasznalok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kolcsonzesek_Foglalasok_FoglalasId",
                table: "Kolcsonzesek");

            migrationBuilder.DropForeignKey(
                name: "FK_Lakcimek_Felhasznalok_FelhasznaloId",
                table: "Lakcimek");

            migrationBuilder.DropIndex(
                name: "IX_Kolcsonzesek_FoglalasId",
                table: "Kolcsonzesek");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lakcimek",
                table: "Lakcimek");

            migrationBuilder.DropIndex(
                name: "IX_Lakcimek_FelhasznaloId",
                table: "Lakcimek");

            migrationBuilder.DropColumn(
                name: "Isbn",
                table: "Konyvek");

            migrationBuilder.DropColumn(
                name: "Kiadasev",
                table: "Konyvek");

            migrationBuilder.DropColumn(
                name: "FoglalasId",
                table: "Kolcsonzesek");

            migrationBuilder.RenameTable(
                name: "Lakcimek",
                newName: "Lakimcek");

            migrationBuilder.AlterColumn<int>(
                name: "Statusz",
                table: "Konyvek",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublikalasIdeje",
                table: "Konyvek",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Kategoria",
                table: "Konyvek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ertelekes",
                table: "Konyvek",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Allapot",
                table: "Konyvek",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FelhasznaloId",
                table: "Lakimcek",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lakimcek",
                table: "Lakimcek",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lakimcek_FelhasznaloId",
                table: "Lakimcek",
                column: "FelhasznaloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lakimcek_Felhasznalok_FelhasznaloId",
                table: "Lakimcek",
                column: "FelhasznaloId",
                principalTable: "Felhasznalok",
                principalColumn: "Id");
        }
    }
}
