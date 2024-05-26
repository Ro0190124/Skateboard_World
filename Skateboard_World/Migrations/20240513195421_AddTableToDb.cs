using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skateboard_World.Migrations
{
    /// <inheritdoc />
    public partial class AddTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "db_NGUOI_DUNG",
                columns: table => new
                {
                    MaND = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenND = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PhanQuyen = table.Column<bool>(type: "bit", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_NGUOI_DUNG", x => x.MaND);
                });

            migrationBuilder.CreateTable(
                name: "db_SAN_PHAM",
                columns: table => new
                {
                    MaSP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaNhap = table.Column<double>(type: "float", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_SAN_PHAM", x => x.MaSP);
                });

            migrationBuilder.CreateTable(
                name: "db_GIO_HANG",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_GIO_HANG", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_db_GIO_HANG_db_NGUOI_DUNG_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "db_NGUOI_DUNG",
                        principalColumn: "MaND",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_DS_HINH_ANH",
                columns: table => new
                {
                    MaDSHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaHinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_DS_HINH_ANH", x => x.MaDSHinhAnh);
                    table.ForeignKey(
                        name: "FK_db_DS_HINH_ANH_db_SAN_PHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "db_SAN_PHAM",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_CHI_TIET_GIO_HANG",
                columns: table => new
                {
                    MaCTGH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSP = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_CHI_TIET_GIO_HANG", x => x.MaCTGH);
                    table.ForeignKey(
                        name: "FK_db_CHI_TIET_GIO_HANG_db_GIO_HANG_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "db_GIO_HANG",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_db_CHI_TIET_GIO_HANG_db_SAN_PHAM_MaSP",
                        column: x => x.MaSP,
                        principalTable: "db_SAN_PHAM",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "db_HOA_DON",
                columns: table => new
                {
                    MaHD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_HOA_DON", x => x.MaHD);
                    table.ForeignKey(
                        name: "FK_db_HOA_DON_db_GIO_HANG_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "db_GIO_HANG",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_db_CHI_TIET_GIO_HANG_MaGioHang",
                table: "db_CHI_TIET_GIO_HANG",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_db_CHI_TIET_GIO_HANG_MaSP",
                table: "db_CHI_TIET_GIO_HANG",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_db_DS_HINH_ANH_MaSP",
                table: "db_DS_HINH_ANH",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_db_GIO_HANG_MaNguoiDung",
                table: "db_GIO_HANG",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_db_HOA_DON_MaGioHang",
                table: "db_HOA_DON",
                column: "MaGioHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "db_CHI_TIET_GIO_HANG");

            migrationBuilder.DropTable(
                name: "db_DS_HINH_ANH");

            migrationBuilder.DropTable(
                name: "db_HOA_DON");

            migrationBuilder.DropTable(
                name: "db_SAN_PHAM");

            migrationBuilder.DropTable(
                name: "db_GIO_HANG");

            migrationBuilder.DropTable(
                name: "db_NGUOI_DUNG");
        }
    }
}
