using Cars.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;

namespace Cars.Controllers
{
    public class CarController : Controller
    {
        private readonly CarsDbContex _contex;
        public CarController(CarsDbContex contex)
        {
            _contex = contex;
        }

        // GET: CarController
        public ActionResult Index()
        {
            return View(_contex.Cars);
        }

        // GET: CarController/Details/5
        public ActionResult Details(int id)
        {
            return View(_contex.Cars.Find(id));
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
            _contex.Cars.Add(car);
            _contex.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_contex.Cars.Find(id));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Car car)
        {
            var CarToEdit = _contex.Cars.Find(id);
            CarToEdit.Brand = car.Brand;
            CarToEdit.Model = car.Model;
            CarToEdit.Type = car.Type;
            CarToEdit.Power = car.Power;
            CarToEdit.Avalibity = car.Avalibity;
            _contex.Update(CarToEdit);
            _contex.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_contex.Cars.Find(id));
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Car car)
        {
            var CarToRemove = _contex.Cars.Find(car.Id);
            _contex.Cars.Remove(CarToRemove);
            _contex.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
