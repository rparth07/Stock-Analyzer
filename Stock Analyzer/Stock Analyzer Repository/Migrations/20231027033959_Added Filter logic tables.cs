using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock_Analyzer_Repository.Migrations
{
  /// <inheritdoc />
  public partial class AddedFilterlogictables : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Filter",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            FilterName = table.Column<string>(type: "nvarchar(450)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Filter", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "FilterCriteria",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            filterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Sequence = table.Column<int>(type: "int", nullable: false),
            FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            ChangeType = table.Column<int>(type: "int", nullable: false),
            LogicalOperator = table.Column<int>(type: "int", nullable: false),
            PeriodType = table.Column<int>(type: "int", nullable: false),
            PeriodValue = table.Column<int>(type: "int", nullable: false)
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
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            FilterCriteriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            CalculationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Value = table.Column<double>(type: "float", nullable: false)
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
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "FilterResult");

      migrationBuilder.DropTable(
          name: "FilterCriteria");

      migrationBuilder.DropTable(
          name: "Filter");
    }
  }
}
