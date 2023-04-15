using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orderDetail_product",
                table: "orderDetail");

            migrationBuilder.DropForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail");

            migrationBuilder.DropColumn(
                name: "bankName",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "staus",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "total",
                table: "transaction");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "transaction",
                newName: "vnp_Amount");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "transaction",
                newName: "vnp_TransactionStatus");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "transaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TerminalID",
                table: "transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "bankCode",
                table: "transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "vnp_ResponseCode",
                table: "transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "vnp_SecureHash",
                table: "transaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "vnpayTranId",
                table: "transaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "fk_orderDetail_product",
                table: "orderDetail",
                column: "productId",
                principalTable: "product",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail",
                column: "orderId",
                principalTable: "userOrder",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orderDetail_product",
                table: "orderDetail");

            migrationBuilder.DropForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "TerminalID",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "bankCode",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "vnp_ResponseCode",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "vnp_SecureHash",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "vnpayTranId",
                table: "transaction");

            migrationBuilder.RenameColumn(
                name: "vnp_TransactionStatus",
                table: "transaction",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "vnp_Amount",
                table: "transaction",
                newName: "type");

            migrationBuilder.AddColumn<string>(
                name: "bankName",
                table: "transaction",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "staus",
                table: "transaction",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.AddColumn<double>(
                name: "total",
                table: "transaction",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "fk_orderDetail_product",
                table: "orderDetail",
                column: "productId",
                principalTable: "product",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_orderDetail_userOrder",
                table: "orderDetail",
                column: "orderId",
                principalTable: "userOrder",
                principalColumn: "id");
        }
    }
}
