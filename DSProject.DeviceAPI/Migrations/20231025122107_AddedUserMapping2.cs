using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSProject.DeviceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserMapping2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_UserMappings_UserId",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMappings",
                table: "UserMappings");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserMappings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMappings",
                table: "UserMappings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_UserMappings_UserId",
                table: "Devices",
                column: "UserId",
                principalTable: "UserMappings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_UserMappings_UserId",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMappings",
                table: "UserMappings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserMappings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMappings",
                table: "UserMappings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_UserMappings_UserId",
                table: "Devices",
                column: "UserId",
                principalTable: "UserMappings",
                principalColumn: "UserId");
        }
    }
}
