using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPYte.Data.Migrations
{
    public partial class appUser_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrative_regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrative_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "administrative_units",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    short_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    short_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrative_units", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userOrder",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total = table.Column<double>(type: "float", nullable: false),
                    grandTotal = table.Column<double>(type: "float", nullable: false),
                    customerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    addressId = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    shipping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shippingVia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userOrder", x => x.id);
                    table.ForeignKey(
                        name: "fk_Order_AspNetUsers",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    administrative_unit_id = table.Column<int>(type: "int", nullable: true),
                    administrative_region_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("provinces_pkey", x => x.code);
                    table.ForeignKey(
                        name: "provinces_administrative_region_id_fkey",
                        column: x => x.administrative_region_id,
                        principalTable: "administrative_regions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "provinces_administrative_unit_id_fkey",
                        column: x => x.administrative_unit_id,
                        principalTable: "administrative_units",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    summary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<double>(type: "float", nullable: false),
                    stock = table.Column<double>(type: "float", nullable: false, defaultValueSql: "((1))"),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    isVisible = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((1))"),
                    status = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((1))"),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    categoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_AspNetUsers",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_product_category",
                        column: x => x.categoryId,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<double>(type: "float", nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total = table.Column<double>(type: "float", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    staus = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    orderId = table.Column<long>(type: "bigint", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_AspNetUsers",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_transaction_userOrder",
                        column: x => x.orderId,
                        principalTable: "userOrder",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    province_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    administrative_unit_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("districts_pkey", x => x.code);
                    table.ForeignKey(
                        name: "districts_administrative_unit_id_fkey",
                        column: x => x.administrative_unit_id,
                        principalTable: "administrative_units",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "districts_province_code_fkey",
                        column: x => x.province_code,
                        principalTable: "provinces",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "orderDetail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    totalPrice = table.Column<double>(type: "float", nullable: false),
                    productId = table.Column<long>(type: "bigint", nullable: false),
                    orderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderDetail", x => x.id);
                    table.ForeignKey(
                        name: "fk_orderDetail_product",
                        column: x => x.productId,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_orderDetail_userOrder",
                        column: x => x.orderId,
                        principalTable: "userOrder",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productImg",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isCover = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    productId = table.Column<long>(type: "bigint", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productImg", x => x.id);
                    table.ForeignKey(
                        name: "fk_productImg_product",
                        column: x => x.productId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wards",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    district_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    administrative_unit_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("wards_pkey", x => x.code);
                    table.ForeignKey(
                        name: "wards_administrative_unit_id_fkey",
                        column: x => x.administrative_unit_id,
                        principalTable: "administrative_units",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "wards_district_code_fkey",
                        column: x => x.district_code,
                        principalTable: "districts",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    addressDetail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(sysdatetime())"),
                    updatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    userId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    wardCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_address_AspNetUsers",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_address_wards",
                        column: x => x.wardCode,
                        principalTable: "wards",
                        principalColumn: "code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_userId",
                table: "address",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_address_wardCode",
                table: "address",
                column: "wardCode");

            migrationBuilder.CreateIndex(
                name: "IX_districts_administrative_unit_id",
                table: "districts",
                column: "administrative_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_districts_province_code",
                table: "districts",
                column: "province_code");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetail_orderId",
                table: "orderDetail",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetail_productId",
                table: "orderDetail",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_product_categoryId",
                table: "product",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_product_userId",
                table: "product",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_productImg_productId",
                table: "productImg",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_provinces_administrative_region_id",
                table: "provinces",
                column: "administrative_region_id");

            migrationBuilder.CreateIndex(
                name: "IX_provinces_administrative_unit_id",
                table: "provinces",
                column: "administrative_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_orderId",
                table: "transaction",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_userId",
                table: "transaction",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userOrder_userId",
                table: "userOrder",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "unq_userOrder_id",
                table: "userOrder",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wards_administrative_unit_id",
                table: "wards",
                column: "administrative_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_wards_district_code",
                table: "wards",
                column: "district_code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "orderDetail");

            migrationBuilder.DropTable(
                name: "productImg");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "wards");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "userOrder");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "provinces");

            migrationBuilder.DropTable(
                name: "administrative_regions");

            migrationBuilder.DropTable(
                name: "administrative_units");
        }
    }
}
