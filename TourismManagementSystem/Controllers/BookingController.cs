using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess;
using Tourism.DataAccess.Models;
using TourismManagementSystem.ViewModels;

namespace TourismManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly TourismDbContext _context;

        public BookingController(TourismDbContext context)
        {
            _context = context;
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create(int? packageId)
        {
            if (packageId == null)
                return NotFound();

            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
                return NotFound();

            // Pre-fill BookingViewModel
            var bookingVM = new BookingViewModel
            {
                PackageId = package.PackageId,
                PackageName = package.Name,
                Location = package.Location,
                Price = package.Price,
                BookingDate = DateTime.Now,
                Status = "Booked"
            };

            return View(bookingVM);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel bookingVM)
        {
            if (!ModelState.IsValid)
                return View(bookingVM);

            var package = await _context.Packages.FindAsync(bookingVM.PackageId);
            if (package == null)
                return NotFound();

            var booking = new Booking
            {
                PackageId = bookingVM.PackageId,
                NumberOfSeats = bookingVM.NumberOfSeats,
                BookingDate = DateTime.Now,
                Status = "Booked"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Create payment record
            var payment = new Payment
            {
                BookingId = booking.BookingId,
                Amount = booking.NumberOfSeats * package.Price,
                PaymentDate = DateTime.Now,
                PaymentStatus = "Success"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(History));
        }

        // GET: /Booking/History
        public IActionResult History()
        {
            var history = _context.Bookings
                .Include(b => b.Package)
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    PackageId = b.PackageId,
                    PackageName = b.Package.Name,
                    Location = b.Package.Location,
                    Price = b.Package.Price,
                    NumberOfSeats = b.NumberOfSeats,
                    BookingDate = b.BookingDate,
                    Status = b.Status,
                    PaymentStatus = _context.Payments
                        .Where(p => p.BookingId == b.BookingId)
                        .Select(p => p.PaymentStatus)
                        .FirstOrDefault() ?? "Not Paid",
                    Amount = _context.Payments
                        .Where(p => p.BookingId == b.BookingId)
                        .Select(p => p.Amount)
                        .FirstOrDefault()
                })
                .ToList();

            return View(history);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null) return NotFound();

            booking.Status = "Cancelled";

            var payment = _context.Payments.FirstOrDefault(p => p.BookingId == booking.BookingId);
            if (payment != null && payment.PaymentStatus == "Success")
            {
                var refundAmount = payment.Amount * 0.85m; // 15% deduction
                payment.PaymentStatus = "Refunded";
                payment.RefundAmount = refundAmount;
            }

            _context.SaveChanges();
            TempData["Message"] = "Booking cancelled successfully. Refund processed with 15% deduction.";
            return RedirectToAction("History");
        }
    }
}
