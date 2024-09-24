using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock_Analyzer_Repository.Migrations
{
  /// <inheritdoc />
  public partial class updatedfiltertabletohavefiltertype : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
          name: "FilterType",
          table: "Filter",
          type: "int",
          nullable: false,
          defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "FilterType",
          table: "Filter");
    }
  }
}
