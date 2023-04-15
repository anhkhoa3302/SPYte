using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class model_change2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail");

            migrationBuilder.DropForeignKey(
                name: "fk_Order_AspNetUsers",
                table: "userOrder");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "userOrder",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "addressId",
                table: "userOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "transaction",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<long>(
                name: "orderId",
                table: "transaction",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "productId",
                table: "orderDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "orderId",
                table: "orderDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail",
                column: "orderId",
                principalTable: "userOrder",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_Order_AspNetUsers",
                table: "userOrder",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail");

            migrationBuilder.DropForeignKey(
                name: "fk_Order_AspNetUsers",
                table: "userOrder");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "userOrder",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "addressId",
                table: "userOrder",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "transaction",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "orderId",
                table: "transaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "productId",
                table: "orderDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "orderId",
                table: "orderDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail",
                column: "orderId",
                principalTable: "userOrder",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_Order_AspNetUsers",
                table: "userOrder",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
