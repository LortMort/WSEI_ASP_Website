using Cars.Migrations;
using Cars.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            return View(_context.Reservations);
        }

        public async Task<IActionResult> Reserve(int carId, string carName)
        {
            ViewBag.CarId = carId;
            ViewBag.CarName = carName;

            var reservationsForCar = await _context.Reservations
                                 .Include(r => r.Car)
                                 .Where(r => r.CarId == carId)
                                 .ToListAsync();


            //var reservationsForCar = _context.Reservations
                //.Where(r => r.CarId == carId)
               // .ToList();

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
                ReturnDate = DateTime.Now.AddHours(1),
        };

            return View(newReservation);
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            int carId = reservation.CarId;
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            string carName = car.Brand + " " + car.Model;

            reservation.UserId = _userManager.GetUserId(User);
            reservation.ReservationDate = DateTime.Now;
            reservation.Car = car;

            bool isOverlapping = _context.Reservations.Any(r =>
            r.CarId == carId && !(r.PickupDate >= reservation.ReturnDate || r.ReturnDate <= reservation.PickupDate));

            if (isOverlapping)
            {
                ModelState.AddModelError("", "There is already a reservation for the selected car in this time period.");
            }

            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyReservations",new {_context.Reservations });
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

        public async Task<IActionResult> MyReservations()
        {
            var userId = _userManager.GetUserId(User); // Get the current user's ID
            var reservations = await _context.Reservations
                                             .Where(r => r.UserId == userId)
                                             .ToListAsync();

            return View(reservations);
        }

        [HttpGet]
        public JsonResult CheckForOverlap(int carId, DateTime pickupDate, DateTime returnDate)
        {
            bool isOverlapping = _context.Reservations.Any(r =>
            r.CarId == carId && !(r.PickupDate >= returnDate || r.ReturnDate <= pickupDate));
            //bool isOverlapping = _context.Reservations.Any(r =>
            //r.CarId == carId &&
            //((r.PickupDate <= pickupDate && r.ReturnDate > pickupDate) ||
            // (r.PickupDate < returnDate && r.ReturnDate >= returnDate)));

            return Json(isOverlapping);
        }

    }
}
