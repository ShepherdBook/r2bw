using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw_alpha.Data.Migrations
{
    public partial class PurchaseTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseType_TypeName",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseType",
                table: "PurchaseType");

            migrationBuilder.RenameTable(
                name: "PurchaseType",
                newName: "PurchaseTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseTypes",
                table: "PurchaseTypes",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseTypes_TypeName",
                table: "Purchases",
                column: "TypeName",
                principalTable: "PurchaseTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseTypes_TypeName",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseTypes",
                table: "PurchaseTypes");

            migrationBuilder.RenameTable(
                name: "PurchaseTypes",
                newName: "PurchaseType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseType",
                table: "PurchaseType",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseType_TypeName",
                table: "Purchases",
                column: "TypeName",
                principalTable: "PurchaseType",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
