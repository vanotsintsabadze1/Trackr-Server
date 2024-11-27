using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trackr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 17, 59, 48, 463, DateTimeKind.Utc).AddTicks(1863),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2024, 11, 25, 20, 44, 17, 761, DateTimeKind.Utc).AddTicks(8230));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 25, 20, 44, 17, 761, DateTimeKind.Utc).AddTicks(8230),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2024, 11, 26, 17, 59, 48, 463, DateTimeKind.Utc).AddTicks(1863));
        }
    }
}
