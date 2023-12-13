using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSProject.MonitoringAPI.Migrations
{
    /// <inheritdoc />
    public partial class added_new_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LastHourComsumption",
                table: "Devices",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastHourComsumption",
                table: "Devices");
        }
    }
}
