using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw_alpha.Data.Migrations
{
    public partial class ParticipantName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
