using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ePOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSortIndexEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "OptionAttributes");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Toppings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "OptionAttributeValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "ItemToppings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "ItemSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "ItemOptionAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Toppings");

            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "OptionAttributeValues");

            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "ItemToppings");

            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "ItemSizes");

            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "ItemOptionAttributes");

            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "OptionAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
