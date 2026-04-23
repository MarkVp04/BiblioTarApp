using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTarApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class MultipleAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lakcimek_FelhasznaloId",
                table: "Lakcimek");

            migrationBuilder.CreateIndex(
                name: "IX_Lakcimek_FelhasznaloId",
                table: "Lakcimek",
                column: "FelhasznaloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lakcimek_FelhasznaloId",
                table: "Lakcimek");

            migrationBuilder.CreateIndex(
                name: "IX_Lakcimek_FelhasznaloId",
                table: "Lakcimek",
                column: "FelhasznaloId",
                unique: true);
        }
    }
}
