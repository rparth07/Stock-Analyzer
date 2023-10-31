using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock_Analyzer_Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFiltertableaddedseriesfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Filter",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Series",
                table: "Filter");
        }
    }
}
