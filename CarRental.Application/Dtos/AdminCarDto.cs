using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class AdminCarDto
  {
    public int CarId { get; set; }

    public int BranchId { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public CarStatus CarStatus { get; set; }

    public VehicleCategories VehicleCategories { get; set; }

    public int NumberOfDoors { get; set; }

    public TransmissionType TransmissionType { get; set; }

    public decimal EngineCapacity { get; set; }

    public bool HasAC { get; set; }

    public decimal PricePerDay { get; set; }
  }

}