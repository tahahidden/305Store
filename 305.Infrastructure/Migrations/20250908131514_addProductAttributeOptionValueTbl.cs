using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace _305.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addProductAttributeOptionValueTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAttributeOptionValue",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productAttributeId = table.Column<long>(type: "bigint", nullable: false),
                    attributeOptionId = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeOptionValue", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptionValue_AttributeOption_attributeOption~",
                        column: x => x.attributeOptionId,
                        principalTable: "AttributeOption",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptionValue_ProductAttribute_productAttribu~",
                        column: x => x.productAttributeId,
                        principalTable: "ProductAttribute",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptionValue_attributeOptionId",
                table: "ProductAttributeOptionValue",
                column: "attributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptionValue_productAttributeId",
                table: "ProductAttributeOptionValue",
                column: "productAttributeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeOptionValue");
        }
    }
}
