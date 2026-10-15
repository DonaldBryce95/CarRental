using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class AdminReservationDto
  {
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int CarId { get; set; }

    public CarStatus CarStatus { get; set; }

    public DateOnly PickUpDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public TimeSpan PickUpTime { get; set; }

    public TimeSpan ReturnTime { get; set; }

    public int PickUpBranchId { get; set; }

    public int ReturnBranchId { get; set; }

    public string PickUpBranchName { get; set; } = string.Empty;

    public string ReturnBranchName { get; set; } = string.Empty;

    public ReservationStatus ReservationStatus { get; set; }

    public decimal TotalPrice { get; set; }
  }
}