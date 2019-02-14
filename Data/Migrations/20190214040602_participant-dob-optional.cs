using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw.Data.Migrations
{
    public partial class participantdoboptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
