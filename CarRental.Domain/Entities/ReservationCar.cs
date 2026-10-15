namespace CarRental.Domain.Entities
{
  public class ReservationCar
  {
    public int ReservationCarId { get; set; }

    public int ReservationId { get; set; }

    public Reservation? Reservation { get; set; }

    public int CarId { get; set; }

    public Car? Car { get; set; }
  }
}