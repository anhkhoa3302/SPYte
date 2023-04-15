using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class appUser_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userDescription",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "middleName",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "lastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)");

            migrationBuilder.AlterColumn<string>(
                name: "firstName",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "avatarPicUrl",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userDescription",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "middleName",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "firstName",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "avatarPicUrl",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);
        }
    }
}
