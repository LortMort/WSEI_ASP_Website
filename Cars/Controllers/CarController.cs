using Cars.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;

namespace Cars.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDbContext _context;
        public CarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CarController
        public ActionResult Index()
        {
            return View(_context.Cars);
        }

        // GET: CarController/Details/5
        public ActionResult Details(int carId)
        {
            return View(_context.Cars.Find(carId));
        }

        // GET: CarController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarController/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int carId)
        {
            return View(_context.Cars.Find(carId));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int carId, Car car)
        {
            var CarToEdit = _context.Cars.Find(carId);
            CarToEdit.Brand = car.Brand;
            CarToEdit.Model = car.Model;
            CarToEdit.Type = car.Type;
            CarToEdit.Power = car.Power;
            CarToEdit.Avalibity = car.Avalibity;
            _context.Update(CarToEdit);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int carId)
        {
            return View(_context.Cars.Find(carId));
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Car car)
        {
            var CarToRemove = _context.Cars.Find(car.Id);
            _context.Cars.Remove(CarToRemove);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
