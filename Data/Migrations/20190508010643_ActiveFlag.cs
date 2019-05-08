using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw.Data.Migrations
{
    public partial class ActiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Purchases",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Participants",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Groups",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Events",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Attendance",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Attendance");
        }
    }
}
