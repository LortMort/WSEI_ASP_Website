using Microsoft.EntityFrameworkCore;
namespace Cars.Models
{
    public class CarsDbContex : DbContext
    {
        public CarsDbContex(DbContextOptions<CarsDbContex> options) :
        base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
    }
}