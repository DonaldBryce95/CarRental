namespace CarRental.Domain.Entities
{
  public class Branch
  {
    public int BranchId { get; set; }

    public string BranchName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string OpeningHours { get; set; } = string.Empty;

    public ICollection<Car> Cars { get; set; } = new List<Car>();

    public ICollection<Reservation> PickupReservations { get; set; } = new List<Reservation>();

    public ICollection<Reservation> ReturnReservations { get; set; } = new List<Reservation>();
  }
}