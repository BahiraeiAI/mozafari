using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoshedTehran.Migrations
{
    /// <inheritdoc />
    public partial class Fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IP",
                table: "Queries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "Queries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
