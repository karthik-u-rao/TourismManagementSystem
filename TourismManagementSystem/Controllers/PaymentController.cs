using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess;
using Tourism.DataAccess.Models;

namespace TourismManagementSystem.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly TourismDbContext _context;

        public PaymentController(TourismDbContext context)
        {
            _context = context;
        }

        // GET: /Payment/Checkout/{bookingId}
        public async Task<IActionResult> Checkout(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Package)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null) return NotFound();

            var payment = new Payment
            {
                BookingId = booking.BookingId,
                Amount = booking.NumberOfSeats * booking.Package.Price
            };

            return View(payment);
        }

        // POST: /Payment/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Payment payment)
        {
            if (ModelState.IsValid)
            {
                // Simulate Payment Success
                payment.PaymentStatus = "Success";
                payment.PaymentDate = DateTime.Now;

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Receipt", new { id = payment.PaymentId });
            }
            return View(payment);
        }

        // GET: /Payment/Receipt/{id}
        public async Task<IActionResult> Receipt(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.Package)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }
    }
}
