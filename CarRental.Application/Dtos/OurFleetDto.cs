using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class OurFleetDto
  {
    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public decimal EngineCapacity { get; set; }

    public VehicleCategories VehicleCategories { get; set; }
  }
}