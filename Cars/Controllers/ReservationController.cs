using Cars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cars.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationsDbContex _context;

        public ReservationController(ReservationsDbContex context)
        {
            _context = context;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            return View(_context.Reservations);
        }

        public IActionResult Reserve(int carId)
        {
            var reservationsForCar = _context.Reservations
                .Include(r => r.Car)
                .Where(r => r.CarId == carId)
                .ToList();

            return View(reservationsForCar);
        }

        // Other actions for reservations (Create, Edit, Delete, etc.) can go here
    }
}
