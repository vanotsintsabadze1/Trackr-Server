using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailConfirmedAttributeToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 19, 13, 9, 31, 351, DateTimeKind.Utc).AddTicks(6405),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2024, 11, 26, 17, 59, 48, 463, DateTimeKind.Utc).AddTicks(1863));

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 17, 59, 48, 463, DateTimeKind.Utc).AddTicks(1863),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2024, 12, 19, 13, 9, 31, 351, DateTimeKind.Utc).AddTicks(6405));
        }
    }
}
