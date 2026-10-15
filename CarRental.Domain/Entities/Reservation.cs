namespace CarRental.Domain.Entities
{
  public class Reservation
  {
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public int CarId { get; set; }

    public Car? Car { get; set; }

    public DateOnly PickUpDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public TimeSpan PickUpTime { get; set; }

    public TimeSpan ReturnTime { get; set; }

    public int PickUpBranchId { get; set; }

    public Branch? PickUpBranch { get; set; }

    public int ReturnBranchId { get; set; }

    public Branch? ReturnBranch { get; set; }

    public ReservationStatus ReservationStatus { get; set; }

    public decimal TotalPrice { get; set; }

    public ICollection<ReservationCar> ReservationCars { get; set; } = new List<ReservationCar>();
  }
}