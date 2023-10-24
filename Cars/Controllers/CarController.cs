using Cars.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Cars.Controllers
{
    public class CarController : Controller
    {
        private static IList<Car> cars = new List<Car>
        {
            new Car() {Id=1, Marka="BMW", Nazwa="E60", Moc=200},
            new Car() {Id=2, Marka="Audi", Nazwa="TT", Moc=175},
            new Car() {Id=3, Marka="Mazda", Nazwa="MX-5", Moc=115}
        };
        // GET: CarController
        public ActionResult Index()
        {
            return View(cars);
        }

        // GET: CarController/Details/5
        public ActionResult Details(int id)
        {
            return View(cars.FirstOrDefault(x => x.Id == id));
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
            car.Id = cars.Count + 1;
            cars.Add(car);
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(cars.FirstOrDefault(x => x.Id == id));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            Car EditCar = cars.FirstOrDefault(x => x.Id == car.Id);
            EditCar.Marka = car.Marka;
            EditCar.Nazwa = car.Nazwa;
            EditCar.Moc = car.Moc;
            return RedirectToAction(nameof(Index));
        }

        // GET: CarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(cars.FirstOrDefault(x => x.Id == id));
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Car car)
        {
            Car CarToDelte = cars.FirstOrDefault(x => x.Id == car.Id);
            cars.Remove(CarToDelte);
            return RedirectToAction(nameof(Index));
        }
    }
}
