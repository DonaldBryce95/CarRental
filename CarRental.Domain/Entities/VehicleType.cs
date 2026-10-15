namespace CarRental.Domain.Entities
{
  public class VehicleType
  {
    public int VehicleTypeId { get; set; }

    public VehicleCategories VehicleCategories { get; set; }

    public decimal PricePerDay { get; set; }

    public ICollection<Car> Cars { get; set; } = new List<Car>();
  }
}