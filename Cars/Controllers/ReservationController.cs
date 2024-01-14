using Cars.Migrations;
using Cars.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var reservations = _context.Reservations.Include(r => r.Car).Include(r => r.User).Where(r => r.ReturnDate > DateTime.Now).ToList();
            
            return View(reservations);
        }

        public async Task<IActionResult> Reserve(int carId, string carName)
        {
            ViewBag.CarId = carId;
            ViewBag.CarName = carName;

            var now = DateTime.Now; // Current date and time
            var oneWeekLater = DateTime.Today.AddDays(8);

            var reservationsForCar = await _context.Reservations
                                          .Include(r => r.Car)
                                          .Where(r => r.CarId == carId
                                                      && r.ReturnDate >= now
                                                      && r.ReturnDate < oneWeekLater)
                                          .ToListAsync();

            return View(reservationsForCar);
        }

        // GET: ReservationController/Create
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            int carId = reservation.CarId;
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            string carName = car.Brand + " " + car.Model;

            reservation.CustomerName = _userManager.GetUserName(User);
            reservation.UserId = _userManager.GetUserId(User);
            reservation.ReservationDate = DateTime.Now;
            reservation.Car = car;

            bool isOverlapping = _context.Reservations.Any(r =>
            r.CarId == carId && ((reservation.PickupDate < r.ReturnDate && reservation.PickupDate >= r.PickupDate) ||
            (reservation.ReturnDate < r.ReturnDate && reservation.ReturnDate >= r.PickupDate)));

            if (isOverlapping)
            {
                ModelState.AddModelError("", "There is already a reservation for the selected car in this time period.");
            }

            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyReservations");
                }
            else
            {
                ViewBag.CarId = carId;
                ViewBag.CarName = carName;

                return View(reservation);
            }
        }

        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            var userId = _userManager.GetUserId(User); // Get the current user's ID
            var now = DateTime.Now; // Current date and time

            var upcomingReservations = await _context.Reservations
                                                     .Include(r => r.Car)
                                                     .Where(r => r.UserId == userId && r.PickupDate >= now)
                                                     .OrderBy(r => r.PickupDate) // Ordering by date might be helpful
                                                     .ToListAsync();

            var pastReservations = await _context.Reservations
                                                 .Include(r => r.Car)
                                                 .Where(r => r.UserId == userId && r.PickupDate < now)
                                                 .OrderByDescending(r => r.PickupDate) // Ordering by date in descending order
                                                 .ToListAsync();

            var model = new MyReservationsViewModel
            {
                UpcomingReservations = upcomingReservations,
                PastReservations = pastReservations
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult CheckForOverlap(int carId, DateTime pickupDate, DateTime returnDate)
        {
            bool isOverlapping = _context.Reservations.Any(r =>
            r.CarId == carId && ((pickupDate < r.ReturnDate && pickupDate >= r.PickupDate) ||
            (returnDate < r.ReturnDate && returnDate >= r.PickupDate)));

            return Json(isOverlapping);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int ReservationId)
        {
            var reservationToDelete = _context.Reservations
                                              .Include(r => r.Car)
                                              .Include(r => r.User)
                                              .FirstOrDefault(r => r.ReservationId == ReservationId);
            System.Diagnostics.Debug.WriteLine($"reservation {ReservationId}");

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Admin");

            if (reservationToDelete.UserId != currentUserId && !isAdmin )
            {
                System.Diagnostics.Debug.WriteLine($"Unauthorized delete attempt by user {currentUserId} on reservation {ReservationId}");
                return Unauthorized();
            }

            return View(reservationToDelete);
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Reservation reservation)
        {
            var ReservationToRemove = _context.Reservations.Find(reservation.ReservationId);
            _context.Reservations.Remove(ReservationToRemove);
            _context.SaveChanges();
            return RedirectToAction(nameof(MyReservations));
        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int ReservationId)
        {
            var reservationToInspect = _context.Reservations
                                                .Include(r => r.Car)
                                                .Include(r => r.User)
                                                .FirstOrDefault(r => r.ReservationId == ReservationId);
            System.Diagnostics.Debug.WriteLine(reservationToInspect);

            return View(reservationToInspect);
        }

    }
}
