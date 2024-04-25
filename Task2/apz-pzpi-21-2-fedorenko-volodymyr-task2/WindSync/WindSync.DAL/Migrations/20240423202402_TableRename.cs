using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WindSync.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_AspNetUsers_UserId",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_Turbine_WindFarm_WindFarmId",
                table: "Turbine");

            migrationBuilder.DropForeignKey(
                name: "FK_TurbineData_Turbine_TurbineId",
                table: "TurbineData");

            migrationBuilder.DropForeignKey(
                name: "FK_WindFarm_AspNetUsers_UserId",
                table: "WindFarm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WindFarm",
                table: "WindFarm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Turbine",
                table: "Turbine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alert",
                table: "Alert");

            migrationBuilder.RenameTable(
                name: "WindFarm",
                newName: "WindFarms");

            migrationBuilder.RenameTable(
                name: "Turbine",
                newName: "Turbines");

            migrationBuilder.RenameTable(
                name: "Alert",
                newName: "Alerts");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "TurbineData",
                newName: "RatedPower");

            migrationBuilder.RenameIndex(
                name: "IX_WindFarm_UserId",
                table: "WindFarms",
                newName: "IX_WindFarms_UserId");

            migrationBuilder.RenameColumn(
                name: "RatedPower",
                table: "Turbines",
                newName: "SweptArea");

            migrationBuilder.RenameIndex(
                name: "IX_Turbine_WindFarmId",
                table: "Turbines",
                newName: "IX_Turbines_WindFarmId");

            migrationBuilder.RenameIndex(
                name: "IX_Alert_UserId",
                table: "Alerts",
                newName: "IX_Alerts_UserId");

            migrationBuilder.AddColumn<double>(
                name: "AirDensity",
                table: "TurbineData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AirPressure",
                table: "TurbineData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AirTemperature",
                table: "TurbineData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WindFarms",
                table: "WindFarms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Turbines",
                table: "Turbines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alerts",
                table: "Alerts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AspNetUsers_UserId",
                table: "Alerts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TurbineData_Turbines_TurbineId",
                table: "TurbineData",
                column: "TurbineId",
                principalTable: "Turbines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turbines_WindFarms_WindFarmId",
                table: "Turbines",
                column: "WindFarmId",
                principalTable: "WindFarms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WindFarms_AspNetUsers_UserId",
                table: "WindFarms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AspNetUsers_UserId",
                table: "Alerts");

            migrationBuilder.DropForeignKey(
                name: "FK_TurbineData_Turbines_TurbineId",
                table: "TurbineData");

            migrationBuilder.DropForeignKey(
                name: "FK_Turbines_WindFarms_WindFarmId",
                table: "Turbines");

            migrationBuilder.DropForeignKey(
                name: "FK_WindFarms_AspNetUsers_UserId",
                table: "WindFarms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WindFarms",
                table: "WindFarms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Turbines",
                table: "Turbines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alerts",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "AirDensity",
                table: "TurbineData");

            migrationBuilder.DropColumn(
                name: "AirPressure",
                table: "TurbineData");

            migrationBuilder.DropColumn(
                name: "AirTemperature",
                table: "TurbineData");

            migrationBuilder.RenameTable(
                name: "WindFarms",
                newName: "WindFarm");

            migrationBuilder.RenameTable(
                name: "Turbines",
                newName: "Turbine");

            migrationBuilder.RenameTable(
                name: "Alerts",
                newName: "Alert");

            migrationBuilder.RenameColumn(
                name: "RatedPower",
                table: "TurbineData",
                newName: "Temperature");

            migrationBuilder.RenameIndex(
                name: "IX_WindFarms_UserId",
                table: "WindFarm",
                newName: "IX_WindFarm_UserId");

            migrationBuilder.RenameColumn(
                name: "SweptArea",
                table: "Turbine",
                newName: "RatedPower");

            migrationBuilder.RenameIndex(
                name: "IX_Turbines_WindFarmId",
                table: "Turbine",
                newName: "IX_Turbine_WindFarmId");

            migrationBuilder.RenameIndex(
                name: "IX_Alerts_UserId",
                table: "Alert",
                newName: "IX_Alert_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WindFarm",
                table: "WindFarm",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Turbine",
                table: "Turbine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alert",
                table: "Alert",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_AspNetUsers_UserId",
                table: "Alert",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turbine_WindFarm_WindFarmId",
                table: "Turbine",
                column: "WindFarmId",
                principalTable: "WindFarm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TurbineData_Turbine_TurbineId",
                table: "TurbineData",
                column: "TurbineId",
                principalTable: "Turbine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WindFarm_AspNetUsers_UserId",
                table: "WindFarm",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
