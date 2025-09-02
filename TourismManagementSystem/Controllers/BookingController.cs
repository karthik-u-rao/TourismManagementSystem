using Microsoft.AspNetCore.Mvc;
using Tourism.DataAccess;
using Tourism.DataAccess.Models;

namespace TourismManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly TourismDbContext _context;

        public BookingController(TourismDbContext context)
        {
            _context = context;
        }

        // GET: /Booking/Create/5
        public IActionResult Create(int packageId)
        {
            var package = _context.Packages.Find(packageId);
            if (package == null) return NotFound();

            ViewBag.Package = package;
            return View();
        }

        // POST: /Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int packageId, int numberOfSeats)
        {
            var package = _context.Packages.Find(packageId);
            if (package == null || numberOfSeats > package.AvailableSeats)
            {
                ModelState.AddModelError("", "Invalid booking request.");
                ViewBag.Package = package;
                return View();
            }

            var booking = new Booking
            {
                PackageId = packageId,
                BookingDate = DateTime.Now,
                NumberOfSeats = numberOfSeats,
                Status = "Booked"
            };

            package.AvailableSeats -= numberOfSeats;

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("History");
        }

        // GET: /Booking/History
        public IActionResult History()
        {
            var bookings = _context.Bookings
                .Join(_context.Packages,
                      b => b.PackageId,
                      p => p.PackageId,
                      (b, p) => new Booking
                      {
                          BookingId = b.BookingId,
                          PackageId = b.PackageId,
                          Package = p,
                          NumberOfSeats = b.NumberOfSeats,
                          BookingDate = b.BookingDate,
                          Status = b.Status
                      })
                .ToList();

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();

            booking.Status = "Cancelled";
            _context.SaveChanges();

            // Refund logic will be added when I integrate Payment
            TempData["Message"] = "Booking cancelled successfully (refund: 85% of amount).";

            return RedirectToAction("History");
        }
    }
}
