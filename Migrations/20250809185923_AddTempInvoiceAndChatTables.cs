using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshi.Migrations
{
    /// <inheritdoc />
    public partial class AddTempInvoiceAndChatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPrice = table.Column<double>(type: "float", nullable: false),
                    CommissionFee = table.Column<double>(type: "float", nullable: false),
                    VisitingFee = table.Column<double>(type: "float", nullable: false),
                    CancellationFee = table.Column<double>(type: "float", nullable: false),
                    WorkerPromotionFee = table.Column<double>(type: "float", nullable: false),
                    ClientPromotionFee = table.Column<double>(type: "float", nullable: false),
                    ClientIndebtednessFee = table.Column<double>(type: "float", nullable: false),
                    ClientTotalPrice = table.Column<double>(type: "float", nullable: false),
                    WorkerTotalPrice = table.Column<double>(type: "float", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true),
                    OrderVisitId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TempInvoices_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TempInvoices_OrderVisits_OrderVisitId",
                        column: x => x.OrderVisitId,
                        principalTable: "OrderVisits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempInvoices_OfferId",
                table: "TempInvoices",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TempInvoices_OrderVisitId",
                table: "TempInvoices",
                column: "OrderVisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "TempInvoices");
        }
    }
}
