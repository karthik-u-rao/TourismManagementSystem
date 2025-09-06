using Microsoft.AspNetCore.Mvc;
using Tourism.DataAccess;
using Tourism.DataAccess.Models;

namespace TourismManagementSystem.Controllers
{
    public class PackageController : Controller
    {
        private readonly TourismDbContext _context;

        public PackageController(TourismDbContext context)
        {
            _context = context;
        }

        // GET: /Package
        public IActionResult Index()
        {
            var packages = _context.Packages.ToList();
            return View(packages);
        }

        // GET: /Package/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Package/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Package package)
        {
            if (ModelState.IsValid)
            {
                _context.Packages.Add(package);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }

        // GET: /Package/Edit/5
        public IActionResult Edit(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null) return NotFound();
            return View(package);
        }

        // POST: /Package/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Package package)
        {
            if (ModelState.IsValid)
            {
                _context.Packages.Update(package);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }

        // GET: /Package/Delete/5
        public IActionResult Delete(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null) return NotFound();
            return View(package);
        }

        // POST: /Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var package = _context.Packages.Find(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Package/Details/5
        public IActionResult Details(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null) return NotFound();
            return View(package);
        }

        // GET: /Package/Search
        public IActionResult Search(string location, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Packages.AsQueryable();

            if (!string.IsNullOrEmpty(location))
                query = query.Where(p => p.Location.Contains(location));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return View("Index", query.ToList()); // reuse Index view
        }
    }
}
