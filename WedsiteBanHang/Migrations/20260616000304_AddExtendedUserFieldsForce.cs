using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WedsiteBanHang.Migrations
{
    /// <inheritdoc />
    public partial class AddExtendedUserFieldsForce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Thêm cột Address vào bảng AspNetUsers
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            // 2. Thêm cột Age vào bảng AspNetUsers
            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            // 3. Thêm cột FullName vào bảng AspNetUsers (nullable: false vì [Required] trong model)
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Lệnh hạ cấp xóa các cột nếu cần quay lại phiên bản cũ
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}