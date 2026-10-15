using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class CreateCarDto
  {
    [Range(1, int.MaxValue)]
    public int BranchId { get; set; }

    [Range(1, int.MaxValue)]
    public int VehicleTypeId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Model { get; set; } = string.Empty;

    [Range(2, 6)]
    public int NumberOfDoors { get; set; }

    [EnumDataType(typeof(TransmissionType))]
    public TransmissionType TransmissionType { get; set; }

    [EnumDataType(typeof(TransmissionType))]
    public CarStatus CarStatus { get; set; }

    [Range(0.01, 10)]
    public decimal EngineCapacity { get; set; }

    public bool HasAC { get; set; }
  }
}