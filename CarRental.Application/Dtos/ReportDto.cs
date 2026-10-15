using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class ReportDto
  {
    public int? MaintenanceWorkerId { get; set; }
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Make { get; set; } = string.Empty;

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

  }
}