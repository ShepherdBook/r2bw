using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw.Data.Migrations
{
    public partial class ParticipantStatusOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Participants",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParticipantStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_StatusId",
                table: "Participants",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_ParticipantStatus_StatusId",
                table: "Participants",
                column: "StatusId",
                principalTable: "ParticipantStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_ParticipantStatus_StatusId",
                table: "Participants");

            migrationBuilder.DropTable(
                name: "ParticipantStatus");

            migrationBuilder.DropIndex(
                name: "IX_Participants_StatusId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Participants");
        }
    }
}
