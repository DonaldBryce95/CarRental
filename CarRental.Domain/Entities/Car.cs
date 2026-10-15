
namespace CarRental.Domain.Entities
{
  public class Car
  {
    public int CarId { get; set; }

    public int BranchId { get; set; }

    public Branch? Branch { get; set; }

    public int VehicleTypeId { get; set; }

    public VehicleType? VehicleType { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int NumberOfDoors { get; set; }

    public TransmissionType TransmissionType { get; set; }

    public CarStatus CarStatus { get; set; }

    public decimal EngineCapacity { get; set; }

    public bool HasAC { get; set; }

    public ICollection<ReservationCar> ReservationCars { get; set; } = new List<ReservationCar>();

    public ICollection<MaintenanceTask> MaintenanceTasks { get; set; } = new List<MaintenanceTask>();
  }
}