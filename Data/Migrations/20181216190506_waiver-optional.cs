using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw_alpha.Data.Migrations
{
    public partial class waiveroptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "WaiverSignedOn",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "WaiverSignedOn",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
