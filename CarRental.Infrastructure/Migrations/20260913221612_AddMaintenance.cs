using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceWorkers",
                columns: table => new
                {
                    MaintenanceWorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceWorkerStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceWorkers", x => x.MaintenanceWorkerId);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceTasks",
                columns: table => new
                {
                    MaintenanceTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceWorkerId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceTaskStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceTasks", x => x.MaintenanceTaskId);
                    table.ForeignKey(
                        name: "FK_MaintenanceTasks_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceTasks_MaintenanceWorkers_MaintenanceWorkerId",
                        column: x => x.MaintenanceWorkerId,
                        principalTable: "MaintenanceWorkers",
                        principalColumn: "MaintenanceWorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceReports",
                columns: table => new
                {
                    MaintenanceReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceTaskId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceWorkerId = table.Column<int>(type: "int", nullable: false),
                    InspectionResult = table.Column<int>(type: "int", nullable: false),
                    MaintenanceReportStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceReports", x => x.MaintenanceReportId);
                    table.ForeignKey(
                        name: "FK_MaintenanceReports_MaintenanceTasks_MaintenanceTaskId",
                        column: x => x.MaintenanceTaskId,
                        principalTable: "MaintenanceTasks",
                        principalColumn: "MaintenanceTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceReports_MaintenanceWorkers_MaintenanceWorkerId",
                        column: x => x.MaintenanceWorkerId,
                        principalTable: "MaintenanceWorkers",
                        principalColumn: "MaintenanceWorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_MaintenanceTaskId",
                table: "MaintenanceReports",
                column: "MaintenanceTaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceReports_MaintenanceWorkerId",
                table: "MaintenanceReports",
                column: "MaintenanceWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTasks_CarId",
                table: "MaintenanceTasks",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceTasks_MaintenanceWorkerId",
                table: "MaintenanceTasks",
                column: "MaintenanceWorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceReports");

            migrationBuilder.DropTable(
                name: "MaintenanceTasks");

            migrationBuilder.DropTable(
                name: "MaintenanceWorkers");
        }
    }
}
