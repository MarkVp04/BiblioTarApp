using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTarApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddBeosztasToFelhasznalo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Beosztas",
                table: "Felhasznalok",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Beosztas",
                table: "Felhasznalok");
        }
    }
}
