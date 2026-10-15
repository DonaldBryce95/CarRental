using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class CarDto
  {
    public int CarId { get; set; }

    public int BranchId { get; set; }

    public int VehicleTypeId { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int NumberOfDoors { get; set; }

    public TransmissionType TransmissionType { get; set; }

    public CarStatus CarStatus { get; set; }

    public decimal EngineCapacity { get; set; }

    public bool HasAC { get; set; }
  }
}