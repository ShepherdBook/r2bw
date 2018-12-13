using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw_alpha.Data.Migrations
{
    public partial class ParticipantWaiver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WaiverSignedOn",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaiverSignedOn",
                table: "Participants");
        }
    }
}
