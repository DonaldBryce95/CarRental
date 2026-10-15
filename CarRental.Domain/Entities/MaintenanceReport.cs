namespace CarRental.Domain.Entities
{
  public class MaintenanceReport
  {
    public int MaintenanceReportId { get; set; }

    public int MaintenanceTaskId { get; set; }

    public MaintenanceWorker? MaintenanceWorker { get; set; }

    public int? MaintenanceWorkerId { get; set; }

    public MaintenanceTask? MaintenanceTask { get; set; }

    public InspectionResult OverallResult { get; set; }

    public InspectionResult Tyres { get; set; }

    public InspectionResult Lights { get; set; }

    public InspectionResult Brakes { get; set; }

    public InspectionResult Engine { get; set; }

    public InspectionResult Battery { get; set; }

    public InspectionResult AirConditioning { get; set; }

    public InspectionResult WarningLights { get; set; }

    public InspectionResult BodyWork { get; set; }

    public InspectionResult Glass { get; set; }

    public InspectionResult InteriorCondition { get; set; }

    public InspectionResult FuelLevel { get; set; }

    public MaintenanceReportStatus MaintenanceReportStatus { get; set; }
  }
}