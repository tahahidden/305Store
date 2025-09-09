using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _305.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class checkMigNeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_Product_productid",
                table: "Attribute");

            migrationBuilder.DropIndex(
                name: "IX_Attribute_productid",
                table: "Attribute");

            migrationBuilder.DropColumn(
                name: "productid",
                table: "Attribute");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "productid",
                table: "Attribute",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_productid",
                table: "Attribute",
                column: "productid");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Product_productid",
                table: "Attribute",
                column: "productid",
                principalTable: "Product",
                principalColumn: "id");
        }
    }
}
