using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientSpecsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSpecifications_Cities_LivingCityId",
                table: "ClientSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_ClientSpecifications_LivingCityId",
                table: "ClientSpecifications");

            migrationBuilder.DropColumn(
                name: "LivingCityId",
                table: "ClientSpecifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LivingCityId",
                table: "ClientSpecifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecifications_LivingCityId",
                table: "ClientSpecifications",
                column: "LivingCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSpecifications_Cities_LivingCityId",
                table: "ClientSpecifications",
                column: "LivingCityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
