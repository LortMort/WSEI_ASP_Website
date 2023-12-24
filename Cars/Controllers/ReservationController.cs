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
        private readonly ReservationsDbContex _reservationsContext;
        private readonly CarsDbContex _carsContext;

        public ReservationController(ReservationsDbContex reservationsContext, CarsDbContex carsContext)
        {
            _reservationsContext = reservationsContext;
            _carsContext = carsContext;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            return View(_reservationsContext.Reservations);
        }

        public IActionResult Reserve(int carId, string carName)
        {
            ViewBag.CarId = carId;
            ViewBag.CarName = carName;

            System.Diagnostics.Debug.WriteLine("LALAALALALLALAWAWAWAWA");
            System.Diagnostics.Debug.WriteLine(carId);

            var reservationsForCar = _reservationsContext.Reservations
                .Include(r => r.Car)
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
                ReservationDate = DateTime.Now,
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
            var car = _carsContext.Cars.FirstOrDefault(c => c.Id == reservation.CarId);
            System.Diagnostics.Debug.WriteLine("LALAALALALLALA");
            System.Diagnostics.Debug.WriteLine(car);
            System.Diagnostics.Debug.WriteLine(car.Brand);
            reservation.Car = car;


            System.Diagnostics.Debug.WriteLine("REZERWACJA");
            System.Diagnostics.Debug.WriteLine(reservation.Car);
            // Check for overlapping reservations
            bool isOverlapping = _reservationsContext.Reservations.Any(r =>
                r.CarId == reservation.CarId &&
                ((r.PickupDate <= reservation.PickupDate && r.ReturnDate > reservation.PickupDate) ||
                 (r.PickupDate < reservation.ReturnDate && r.ReturnDate >= reservation.ReturnDate)));

            if (isOverlapping)
            {
                System.Diagnostics.Debug.WriteLine("overlap");
                ModelState.AddModelError("", "There is already a reservation for the selected car in this time period.");
            }

            if (ModelState.IsValid)
            {
                _reservationsContext.Reservations.Add(reservation);
                _reservationsContext.SaveChanges();
                System.Diagnostics.Debug.WriteLine("hahaha");
                return RedirectToAction("Index");
            }
            var errors = new StringBuilder();
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.AppendLine(error.ErrorMessage);
                    // You can also log the errors
                    // Debug.WriteLine(error.ErrorMessage);
                }
            }
            System.Diagnostics.Debug.WriteLine("dadadada");
            System.Diagnostics.Debug.WriteLine(errors);
            System.Diagnostics.Debug.WriteLine(ModelState.ErrorCount);
            System.Diagnostics.Debug.WriteLine(ModelState);
            System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
            System.Diagnostics.Debug.WriteLine(ModelState.ToString());
            return View(reservation);
        }

    }
}
