using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class InspectionResultDto
  {
    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult OverallResult { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult Tyres { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult Lights { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult Brakes { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult Engine { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult Battery { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult AirConditioning { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult WarningLights { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult BodyWork { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult Glass { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult InteriorCondition { get; set; }

    [EnumDataType(typeof(InspectionResult))]
    public InspectionResult FuelLevel { get; set; }

  }
}