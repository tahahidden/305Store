using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _305.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixDataBaseLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_Product_productId",
                table: "Attribute");

            migrationBuilder.DropColumn(
                name: "isRequired",
                table: "Attribute");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "Attribute",
                newName: "productid");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_productId",
                table: "Attribute",
                newName: "IX_Attribute_productid");

            migrationBuilder.AlterColumn<long>(
                name: "productid",
                table: "Attribute",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "CategoryProductRelation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    productId = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProductRelation", x => x.id);
                    table.ForeignKey(
                        name: "FK_CategoryProductRelation_ProductCategory_productCategoryId",
                        column: x => x.productCategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProductRelation_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productId = table.Column<long>(type: "bigint", nullable: false),
                    attributeId = table.Column<long>(type: "bigint", nullable: false),
                    isRequired = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductAttribute_Attribute_attributeId",
                        column: x => x.attributeId,
                        principalTable: "Attribute",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAttribute_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProductRelation_productCategoryId",
                table: "CategoryProductRelation",
                column: "productCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProductRelation_productId",
                table: "CategoryProductRelation",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_attributeId",
                table: "ProductAttribute",
                column: "attributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_productId",
                table: "ProductAttribute",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Product_productid",
                table: "Attribute",
                column: "productid",
                principalTable: "Product",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attribute_Product_productid",
                table: "Attribute");

            migrationBuilder.DropTable(
                name: "CategoryProductRelation");

            migrationBuilder.DropTable(
                name: "ProductAttribute");

            migrationBuilder.RenameColumn(
                name: "productid",
                table: "Attribute",
                newName: "productId");

            migrationBuilder.RenameIndex(
                name: "IX_Attribute_productid",
                table: "Attribute",
                newName: "IX_Attribute_productId");

            migrationBuilder.AlterColumn<long>(
                name: "productId",
                table: "Attribute",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isRequired",
                table: "Attribute",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Product_productId",
                table: "Attribute",
                column: "productId",
                principalTable: "Product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
