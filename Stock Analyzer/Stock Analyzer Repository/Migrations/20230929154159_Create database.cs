using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock_Analyzer_Repository.Migrations
{
  /// <inheritdoc />
  public partial class Createdatabase : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Client",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Client", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Company",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Company", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "BhavCopyInfo",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Series = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Date = table.Column<DateTime>(type: "datetime2", nullable: false),
            CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            PreviousClose = table.Column<double>(type: "float", nullable: false),
            OpenPrice = table.Column<double>(type: "float", nullable: false),
            HighPrice = table.Column<double>(type: "float", nullable: false),
            LowPrice = table.Column<double>(type: "float", nullable: false),
            LastPrice = table.Column<double>(type: "float", nullable: false),
            ClosePrice = table.Column<double>(type: "float", nullable: false),
            AvgPrice = table.Column<double>(type: "float", nullable: false),
            TtlTrdQnty = table.Column<double>(type: "float", nullable: false),
            TurnOverLacs = table.Column<double>(type: "float", nullable: false),
            NoOfTrades = table.Column<double>(type: "float", nullable: false),
            DeliveryQty = table.Column<double>(type: "float", nullable: false),
            DeliveryPercentage = table.Column<double>(type: "float", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_BhavCopyInfo", x => x.Id);
            table.ForeignKey(
                      name: "FK_BhavCopyInfo_Company_CompanyId",
                      column: x => x.CompanyId,
                      principalTable: "Company",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "BulkDeal",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            DealDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            CompanyFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            StockAction = table.Column<int>(type: "int", nullable: false),
            Quantity = table.Column<long>(type: "bigint", nullable: false),
            TradePrice = table.Column<double>(type: "float", nullable: false),
            Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_BulkDeal", x => x.Id);
            table.ForeignKey(
                      name: "FK_BulkDeal_Client_ClientId",
                      column: x => x.ClientId,
                      principalTable: "Client",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_BulkDeal_Company_CompanyId",
                      column: x => x.CompanyId,
                      principalTable: "Company",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_BhavCopyInfo_CompanyId",
          table: "BhavCopyInfo",
          column: "CompanyId");

      migrationBuilder.CreateIndex(
          name: "IX_BulkDeal_ClientId",
          table: "BulkDeal",
          column: "ClientId");

      migrationBuilder.CreateIndex(
          name: "IX_BulkDeal_CompanyId",
          table: "BulkDeal",
          column: "CompanyId");

      migrationBuilder.CreateIndex(
          name: "IX_Client_Name",
          table: "Client",
          column: "Name",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Company_Symbol",
          table: "Company",
          column: "Symbol",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "BhavCopyInfo");

      migrationBuilder.DropTable(
          name: "BulkDeal");

      migrationBuilder.DropTable(
          name: "Client");

      migrationBuilder.DropTable(
          name: "Company");
    }
  }
}
