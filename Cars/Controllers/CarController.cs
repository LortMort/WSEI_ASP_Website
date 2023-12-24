using Cars.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Details(int id)
        {
            return View(_context.Cars.Find(id));
        }

        // GET: CarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_context.Cars.Find(id));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Car car)
        {
            var CarToEdit = _context.Cars.Find(id);
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
        public ActionResult Delete(int id)
        {
            return View(_context.Cars.Find(id));
        }

        // POST: CarController/Delete/5
        [HttpPost]
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
