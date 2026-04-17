using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingSite.Migrations
{
    /// <inheritdoc />
    public partial class CloudinaryIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                schema: "public",
                table: "Photos",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                schema: "public",
                table: "Photos",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                schema: "public",
                table: "Photos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                schema: "public",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                schema: "public",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "public",
                table: "Photos",
                newName: "Path");
        }
    }
}
