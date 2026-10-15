using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos
{
  public class CalculateBookingDto
  {
    [Range(1, int.MaxValue)]
    public int CarId { get; set; }

    public DateOnly PickUpDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public TimeSpan PickUpTime { get; set; }

    public TimeSpan ReturnTime { get; set; }

  }
}