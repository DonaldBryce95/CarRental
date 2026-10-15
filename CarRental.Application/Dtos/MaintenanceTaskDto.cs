using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class MaintenanceTaskDto
  {
    public int MaintenanceTaskId { get; set; }

    public int CarId { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int? MaintenanceWorkerId { get; set; }

    public MaintenanceTaskStatus MaintenanceTaskStatus { get; set; }

    public InspectionResult InspectionResult { get; set; }

    public MaintenanceReportStatus MaintenanceReportStatus { get; set; }
  }
}