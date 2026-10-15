using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos
{
  public class CarReturnedDto
  {
    [Range(1, int.MaxValue)]
    public int CarId { get; set; }
  }
}