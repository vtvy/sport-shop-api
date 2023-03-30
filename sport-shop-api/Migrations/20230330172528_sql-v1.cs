using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sport_shop_api.Migrations
{
    /// <inheritdoc />
    public partial class sqlv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quality",
                table: "Product",
                newName: "Quantity");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Product",
                newName: "Quality");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Product",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
