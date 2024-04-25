using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindSync.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "WindFarms");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "WindFarms");

            migrationBuilder.AddColumn<double>(
                name: "Altitude",
                table: "Turbines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CutInWindSpeed",
                table: "Turbines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Efficiency",
                table: "Turbines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Turbines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Turbines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RatedWindSpeed",
                table: "Turbines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShutDownWindSpeed",
                table: "Turbines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TurbineRadius",
                table: "Turbines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "CutInWindSpeed",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "Efficiency",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "RatedWindSpeed",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "ShutDownWindSpeed",
                table: "Turbines");

            migrationBuilder.DropColumn(
                name: "TurbineRadius",
                table: "Turbines");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "WindFarms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "WindFarms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
