using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw.Data.Migrations
{
    public partial class ParticipantStatusRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_ParticipantStatus_StatusId",
                table: "Participants");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_ParticipantStatus_StatusId",
                table: "Participants",
                column: "StatusId",
                principalTable: "ParticipantStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_ParticipantStatus_StatusId",
                table: "Participants");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_ParticipantStatus_StatusId",
                table: "Participants",
                column: "StatusId",
                principalTable: "ParticipantStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
