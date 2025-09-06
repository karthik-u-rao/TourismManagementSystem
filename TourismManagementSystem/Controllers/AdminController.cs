using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Tourism.DataAccess;
using TourismManagementSystem.ViewModels;

namespace TourismManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TourismDbContext _context;

        public AdminController(TourismDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            var allBookings = _context.Bookings
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    PackageName = b.Package.Name,
                    Location = b.Package.Location,
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

            // Summary stats
            ViewBag.TotalRevenue = _context.Payments.Where(p => p.PaymentStatus == "Success").Sum(p => (decimal?)p.Amount) ?? 0;
            ViewBag.TotalRefunds = _context.Payments.Where(p => p.PaymentStatus == "Refunded").Sum(p => (decimal?)p.RefundAmount) ?? 0;
            ViewBag.TotalBookings = _context.Bookings.Count();
            ViewBag.ActiveBookings = _context.Bookings.Count(b => b.Status == "Booked");

            // Chart 1: Bookings per Package
            var bookingStats = _context.Bookings
                .GroupBy(b => b.Package.Name)
                .Select(g => new { PackageName = g.Key, Count = g.Count() })
                .ToList();

            ViewBag.ChartLabels = bookingStats.Select(x => x.PackageName).ToArray();
            ViewBag.ChartData = bookingStats.Select(x => x.Count).ToArray();

            // Chart 2: Revenue over Time
            var revenueStats = _context.Payments
                .Where(p => p.PaymentStatus == "Success")
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(p => p.Amount) })
                .OrderBy(x => x.Date)
                .ToList();

            ViewBag.RevenueDates = revenueStats.Select(x => x.Date.ToString("yyyy-MM-dd")).ToArray();
            ViewBag.RevenueValues = revenueStats.Select(x => x.Total).ToArray();

            return View(allBookings);
        }


    }
}
