using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock_Analyzer_Repository.Migrations
{
  /// <inheritdoc />
  public partial class addednotebooktable : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Notebook",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
            ContentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Notebook", x => x.Id);
          });

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
          name: "Notebook");
    }
  }
}
