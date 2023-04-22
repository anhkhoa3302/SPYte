using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class new1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_category_product",
                table: "product_category");

            migrationBuilder.AddForeignKey(
                name: "fk_product_category_product",
                table: "product_category",
                column: "product_id",
                principalTable: "product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_category_product",
                table: "product_category");

            migrationBuilder.AddForeignKey(
                name: "fk_product_category_product",
                table: "product_category",
                column: "product_id",
                principalTable: "product",
                principalColumn: "id");
        }
    }
}
