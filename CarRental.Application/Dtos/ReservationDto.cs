using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class ReservationDto
  {
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    public int CarId { get; set; }

    public DateOnly PickUpDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public TimeSpan PickUpTime { get; set; }

    public TimeSpan ReturnTime { get; set; }

    public int PickUpBranchId { get; set; }

    public int ReturnBranchId { get; set; }

    public ReservationStatus ReservationStatus { get; set; }

    public decimal TotalPrice { get; set; }
  }
}