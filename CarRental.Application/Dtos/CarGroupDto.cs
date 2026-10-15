using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class CarGroupDto
  {
    public int CarId { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public VehicleCategories VehicleCategories { get; set; }

    public int NumberOfDoors { get; set; }

    public TransmissionType TransmissionType { get; set; }

    public decimal EngineCapacity { get; set; }

    public bool HasAC { get; set; }
  }
}