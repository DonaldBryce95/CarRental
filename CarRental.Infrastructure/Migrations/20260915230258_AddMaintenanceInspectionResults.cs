using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceInspectionResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InspectionResult",
                table: "MaintenanceReports",
                newName: "WarningLights");

            migrationBuilder.AddColumn<int>(
                name: "AirConditioning",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Battery",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BodyWork",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Brakes",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Engine",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FuelLevel",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Glass",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InteriorCondition",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lights",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OverallResult",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tyres",
                table: "MaintenanceReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirConditioning",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "Battery",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "BodyWork",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "Brakes",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "Engine",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "FuelLevel",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "Glass",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "InteriorCondition",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "Lights",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "OverallResult",
                table: "MaintenanceReports");

            migrationBuilder.DropColumn(
                name: "Tyres",
                table: "MaintenanceReports");

            migrationBuilder.RenameColumn(
                name: "WarningLights",
                table: "MaintenanceReports",
                newName: "InspectionResult");
        }
    }
}
