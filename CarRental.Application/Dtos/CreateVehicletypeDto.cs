using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class CreateVehicleTypeDto
  {
    public VehicleCategories VehicleCategories { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal PricePerDay { get; set; }
  }
}