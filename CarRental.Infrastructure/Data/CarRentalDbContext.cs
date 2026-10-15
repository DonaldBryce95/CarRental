using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Data
{
  public class CarRentalDbContext : IdentityDbContext<ApplicationUser>
  {
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; } = null!;

    public DbSet<Car> Cars { get; set; } = null!;

    public DbSet<Reservation> Reservations { get; set; } = null!;

    public DbSet<ReservationCar> ReservationCars { get; set; } = null!;

    public DbSet<VehicleType> VehicleTypes { get; set; } = null!;

    public DbSet<Branch> Branches { get; set; } = null!;

    public DbSet<MaintenanceTask> MaintenanceTasks { get; set; } = null!;

    public DbSet<MaintenanceWorker> MaintenanceWorkers { get; set; } = null!;

    public DbSet<MaintenanceReport> MaintenanceReports { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<ApplicationUser>().HasOne(u => u.Customer).WithOne().HasForeignKey<ApplicationUser>(u => u.CustomerId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Car>().HasMany(c => c.ReservationCars).WithOne(rc => rc.Car).HasForeignKey(rc => rc.CarId).OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Car>().Property(c => c.EngineCapacity).HasPrecision(10, 2);

      modelBuilder.Entity<Car>().HasMany(c => c.MaintenanceTasks).WithOne(cm => cm.Car).HasForeignKey(m => m.CarId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Branch>().HasMany(b => b.Cars).WithOne(bc => bc.Branch).HasForeignKey(bc => bc.BranchId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Customer>().HasMany(c => c.Reservations).WithOne(rc => rc.Customer).HasForeignKey(rc => rc.CustomerId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Reservation>().HasMany(r => r.ReservationCars).WithOne(rr => rr.Reservation).HasForeignKey(rr => rr.ReservationId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<VehicleType>().HasMany(v => v.Cars).WithOne(vc => vc.VehicleType).HasForeignKey(vc => vc.VehicleTypeId).OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<VehicleType>().Property(v => v.PricePerDay).HasPrecision(18, 2);

      modelBuilder.Entity<Reservation>().HasOne(r => r.PickUpBranch).WithMany(b => b.PickupReservations).HasForeignKey(r => r.PickUpBranchId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Reservation>().HasOne(r => r.ReturnBranch).WithMany(b => b.ReturnReservations).HasForeignKey(r => r.ReturnBranchId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Reservation>().Property(r => r.TotalPrice).HasPrecision(18, 2);

      modelBuilder.Entity<MaintenanceWorker>().HasMany(m => m.MaintenanceTasks).WithOne(mm => mm.MaintenanceWorker).HasForeignKey(m => m.MaintenanceWorkerId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<MaintenanceTask>().HasOne(m => m.MaintenanceReport).WithOne(mm => mm.MaintenanceTask).HasForeignKey<MaintenanceReport>(mm => mm.MaintenanceTaskId).OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<MaintenanceWorker>().HasMany(m => m.MaintenanceReports).WithOne(mm => mm.MaintenanceWorker).HasForeignKey(m => m.MaintenanceWorkerId).OnDelete(DeleteBehavior.Restrict);
    }
  }
}