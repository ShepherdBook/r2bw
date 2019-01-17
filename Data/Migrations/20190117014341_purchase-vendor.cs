using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw_alpha.Data.Migrations
{
    public partial class purchasevendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Purchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Purchases");
        }
    }
}
