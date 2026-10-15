using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class OnHireDto
  {
    public int ReservationId { get; set; }

    public int CarId { get; set; }

    public CarStatus CarStatus { get; set; }
  }
}