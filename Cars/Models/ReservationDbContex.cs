using Cars.Models;
using Microsoft.EntityFrameworkCore;
namespace Cars.Models
{
    public class ReservationsDbContex: DbContext
    {
        public ReservationsDbContex(DbContextOptions<ReservationsDbContex> options) : base(options)
        {
        }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
