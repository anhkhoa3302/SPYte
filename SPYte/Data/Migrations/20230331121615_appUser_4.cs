using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class appUser_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "bannedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "bannedTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "AspNetUsers",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(sysdatetime())");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bannedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "bannedTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "updatedDate",
                table: "AspNetUsers");
        }
    }
}
