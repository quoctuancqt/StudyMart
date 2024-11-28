using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyMart.ApiService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Quantity_Column_In_ShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingCarts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
