using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoExchange.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderBook",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Ticker = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceQuoteAsk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    SumVolume = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    OrderBookId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceQuoteAsk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceQuoteAsk_OrderBook_OrderBookId",
                        column: x => x.OrderBookId,
                        principalTable: "OrderBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceQuoteBid",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    SumVolume = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    OrderBookId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceQuoteBid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceQuoteBid_OrderBook_OrderBookId",
                        column: x => x.OrderBookId,
                        principalTable: "OrderBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuoteAsk_OrderBookId",
                table: "PriceQuoteAsk",
                column: "OrderBookId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceQuoteBid_OrderBookId",
                table: "PriceQuoteBid",
                column: "OrderBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceQuoteAsk");

            migrationBuilder.DropTable(
                name: "PriceQuoteBid");

            migrationBuilder.DropTable(
                name: "OrderBook");
        }
    }
}
