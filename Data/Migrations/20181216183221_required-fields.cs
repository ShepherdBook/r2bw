using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace r2bw.Data.Migrations
{
    public partial class requiredfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "WaiverSignedOn",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Participants",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Participants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Participants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Participants");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "WaiverSignedOn",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
