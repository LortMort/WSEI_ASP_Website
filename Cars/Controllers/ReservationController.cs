using Cars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cars.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            return View(_context.Reservations);
        }

        public IActionResult Reserve(int carId, string carName)
        {
            ViewBag.CarId = carId;
            ViewBag.CarName = carName;

            var reservationsForCar = _context.Reservations
                .Where(r => r.CarId == carId)
                .ToList();

            return View(reservationsForCar);
        }

        // GET: ReservationController/Create
        public ActionResult Create(int carId, string carName)
        {
            ViewBag.CarName = carName;
            ViewBag.CarId = carId;

            var newReservation = new Reservation
            {
                CarId = carId,
                PickupDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddHours(1)
            };

            return View(newReservation);
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation)
        {
            int carId = reservation.CarId;
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            string carName = car.Brand + " " + car.Model;

            reservation.ReservationDate= DateTime.Now;

            bool isOverlapping = _context.Reservations.Any(r =>
            r.CarId == carId && !(r.PickupDate >= reservation.ReturnDate || r.ReturnDate <= reservation.PickupDate));

            if (isOverlapping)
            {
                ModelState.AddModelError("", "There is already a reservation for the selected car in this time period.");
            }

            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return RedirectToAction("Reserve", new { carId = carId, carName = carName });
                }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                ViewBag.CarId = carId;
                ViewBag.CarName = carName;

                return View(reservation);
            }
        }

        [HttpGet]
        public JsonResult CheckForOverlap(int carId, DateTime pickupDate, DateTime returnDate)
        {
            bool isOverlapping = _context.Reservations.Any(r =>
                r.CarId == carId &&
                ((r.PickupDate <= pickupDate && r.ReturnDate > pickupDate) ||
                 (r.PickupDate < returnDate && r.ReturnDate >= returnDate)));

            return Json(isOverlapping);
        }

    }
}
