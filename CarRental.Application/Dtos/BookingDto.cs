namespace CarRental.Application.Dtos
{
  public class BookingDto
  {
    public int CarId { get; set; }

    public DateOnly PickUpDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public TimeSpan PickUpTime { get; set; }

    public TimeSpan ReturnTime { get; set; }

    public int NumberOfDays { get; set; }

    public decimal PricePerDay { get; set; }

    public decimal TotalPrice { get; set; }
  }
}