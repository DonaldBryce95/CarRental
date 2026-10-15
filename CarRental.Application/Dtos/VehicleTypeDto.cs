using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class VehicleTypeDto
  {
    public int VehicleTypeId { get; set; }

    public VehicleCategories VehicleCategories { get; set; }

    public decimal PricePerDay { get; set; }
  }
}