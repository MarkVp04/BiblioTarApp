using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTarApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddKolcsonozhetoToKonyv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Kolcsonozheto",
                table: "Konyvek",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kolcsonozheto",
                table: "Konyvek");
        }
    }
}
