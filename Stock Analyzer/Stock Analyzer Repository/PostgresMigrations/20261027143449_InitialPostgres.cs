using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Stock_Analyzer_Repository.PostgresMigrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FilterName = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: false),
                    FilterType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    TradingDate = table.Column<DateTime>(type: "date", nullable: false),
                    ImportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notebook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ContentDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BhavCopyInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousClose = table.Column<double>(type: "double precision", nullable: false),
                    OpenPrice = table.Column<double>(type: "double precision", nullable: false),
                    HighPrice = table.Column<double>(type: "double precision", nullable: false),
                    LowPrice = table.Column<double>(type: "double precision", nullable: false),
                    LastPrice = table.Column<double>(type: "double precision", nullable: false),
                    ClosePrice = table.Column<double>(type: "double precision", nullable: false),
                    AvgPrice = table.Column<double>(type: "double precision", nullable: false),
                    TtlTrdQnty = table.Column<double>(type: "double precision", nullable: false),
                    TurnOverLacs = table.Column<double>(type: "double precision", nullable: false),
                    NoOfTrades = table.Column<double>(type: "double precision", nullable: false),
                    DeliveryQty = table.Column<double>(type: "double precision", nullable: true),
                    DeliveryPercentage = table.Column<double>(type: "double precision", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    DealDate = table.Column<DateTime>(type: "date", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyFullName = table.Column<string>(type: "text", nullable: false),
                    StockAction = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    TradePrice = table.Column<double>(type: "double precision", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "FilterCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    filterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: false),
                    ChangeType = table.Column<int>(type: "integer", nullable: false),
                    LogicalOperator = table.Column<int>(type: "integer", nullable: false),
                    PeriodType = table.Column<int>(type: "integer", nullable: false),
                    PeriodValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterCriteria_Filter_filterId",
                        column: x => x.filterId,
                        principalTable: "Filter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilterResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilterCriteriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CalculationDate = table.Column<DateTime>(type: "date", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilterResult_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilterResult_FilterCriteria_FilterCriteriaId",
                        column: x => x.FilterCriteriaId,
                        principalTable: "FilterCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BhavCopyInfo_CompanyId_Series_Date",
                table: "BhavCopyInfo",
                columns: new[] { "CompanyId", "Series", "Date" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Filter_FilterName",
                table: "Filter",
                column: "FilterName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilterCriteria_filterId",
                table: "FilterCriteria",
                column: "filterId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterResult_CompanyId",
                table: "FilterResult",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterResult_FilterCriteriaId",
                table: "FilterResult",
                column: "FilterCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatches_Source_TradingDate",
                table: "ImportBatches",
                columns: new[] { "Source", "TradingDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notebook_ContentDate",
                table: "Notebook",
                column: "ContentDate",
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
                name: "FilterResult");

            migrationBuilder.DropTable(
                name: "ImportBatches");

            migrationBuilder.DropTable(
                name: "Notebook");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "FilterCriteria");

            migrationBuilder.DropTable(
                name: "Filter");
        }
    }
}
