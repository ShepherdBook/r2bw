using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw.Data.Migrations
{
    public partial class ParticipantEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseTypes_TypeName",
                table: "Purchases");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Purchases",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseTypes_TypeName",
                table: "Purchases",
                column: "TypeName",
                principalTable: "PurchaseTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseTypes_TypeName",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Participants");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Purchases",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseTypes_TypeName",
                table: "Purchases",
                column: "TypeName",
                principalTable: "PurchaseTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
