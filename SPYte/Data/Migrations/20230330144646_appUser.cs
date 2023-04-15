using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class appUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatarPicUrl",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "middleName",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "userDescription",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatarPicUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "middleName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userDescription",
                table: "AspNetUsers");
        }
    }
}
