using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess;
using Tourism.DataAccess.Models;
using TourismManagementSystem.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace TourismManagementSystem.Controllers
{
    [Authorize] // Require authentication for all booking operations
    public class BookingController : Controller
    {
        private readonly TourismDbContext _context;

        public BookingController(TourismDbContext context)
        {
            _context = context;
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create(int? id)
        {
            System.Diagnostics.Debug.WriteLine($"=== BOOKING CREATE GET ===");
            System.Diagnostics.Debug.WriteLine($"Package ID: {id}");

            if (id == null)
            {
                TempData["Error"] = "Package ID is required.";
                return RedirectToAction("Index", "Package");
            }

            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                TempData["Error"] = "Package not found.";
                return RedirectToAction("Index", "Package");
            }

            System.Diagnostics.Debug.WriteLine($"Package found: {package.Name}");

            // Pass package info to view via ViewBag
            ViewBag.PackageId = package.PackageId;
            ViewBag.PackageName = package.Name;
            ViewBag.PackageLocation = package.Location;
            ViewBag.PackagePrice = package.Price;
            ViewBag.PackageImage = package.ImageUrl ?? "/images/default-package.jpg";

            // Create empty booking model with proper initialization
            var booking = new Booking
            {
                PackageId = package.PackageId,
                BookingDate = DateTime.Now,
                NumberOfSeats = 1,
                Status = "Pending", // Initial status
                CustomerName = string.Empty,
                Email = string.Empty,
                PhoneNumber = string.Empty
            };

            System.Diagnostics.Debug.WriteLine($"Booking model created with PackageId: {booking.PackageId}");

            return View(booking);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            System.Diagnostics.Debug.WriteLine($"=== BOOKING SUBMISSION ===");
            System.Diagnostics.Debug.WriteLine($"PackageId: {booking.PackageId}");
            System.Diagnostics.Debug.WriteLine($"CustomerName: '{booking.CustomerName}'");
            System.Diagnostics.Debug.WriteLine($"Email: '{booking.Email}'");
            System.Diagnostics.Debug.WriteLine($"PhoneNumber: '{booking.PhoneNumber}'");
            System.Diagnostics.Debug.WriteLine($"NumberOfSeats: {booking.NumberOfSeats}");

            // Get package info first
            var package = await _context.Packages.FindAsync(booking.PackageId);
            if (package == null)
            {
                TempData["Error"] = "Package not found.";
                return RedirectToAction("Index", "Package");
            }

            // Set ViewBag data for view reload
            ViewBag.PackageId = package.PackageId;
            ViewBag.PackageName = package.Name;
            ViewBag.PackageLocation = package.Location;
            ViewBag.PackagePrice = package.Price;
            ViewBag.PackageImage = package.ImageUrl ?? "/images/default-package.jpg";

            // Remove model state entries that might cause issues
            ModelState.Remove("Package");
            ModelState.Remove("Payments");
            ModelState.Remove("BookingId");

            // Use proper email validation
            if (!string.IsNullOrWhiteSpace(booking.Email))
            {
                var emailAttribute = new EmailAddressAttribute();
                if (!emailAttribute.IsValid(booking.Email))
                {
                    ModelState.AddModelError("Email", "Please enter a valid email address");
                }
            }

            // Business validation for seat availability
            if (package.AvailableSeats < booking.NumberOfSeats)
            {
                ModelState.AddModelError("NumberOfSeats", $"Only {package.AvailableSeats} seats available");
            }

            // If there are validation errors, display them
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value?.Errors.Select(e => e.ErrorMessage) ?? new List<string>())
                    .ToList();
                
                TempData["Error"] = "Please fix the following errors: " + string.Join(", ", validationErrors);
                System.Diagnostics.Debug.WriteLine($"Validation errors: {string.Join(", ", validationErrors)}");
                return View(booking);
            }

            try
            {
                // Set booking details
                booking.BookingDate = DateTime.Now;
                booking.Status = "Booked";

                System.Diagnostics.Debug.WriteLine($"Attempting to save booking...");

                // Save booking
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Booking saved with ID: {booking.BookingId}");

                // Update available seats
                package.AvailableSeats -= booking.NumberOfSeats;
                _context.Packages.Update(package);

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

                System.Diagnostics.Debug.WriteLine($"Payment created successfully");

                TempData["Success"] = $"Booking confirmed! Your booking ID is #{booking.BookingId}";
                return RedirectToAction(nameof(Confirmation), new { id = booking.BookingId });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                
                TempData["Error"] = "Unable to process booking. Please try again. Error: " + ex.Message;
                return View(booking);
            }
        }

        // GET: /Booking/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Package)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == booking.BookingId);

            var viewModel = new BookingViewModel
            {
                BookingId = booking.BookingId,
                PackageId = booking.PackageId,
                PackageName = booking.Package.Name,
                Location = booking.Package.Location,
                Price = booking.Package.Price,
                NumberOfSeats = booking.NumberOfSeats,
                BookingDate = booking.BookingDate,
                Status = booking.Status,
                CustomerName = booking.CustomerName,
                Email = booking.Email,
                PhoneNumber = booking.PhoneNumber,
                PaymentStatus = payment?.PaymentStatus ?? "Not Paid",
                Amount = payment?.Amount ?? 0,
                ImageUrl = booking.Package.ImageUrl ?? "https://images.unsplash.com/photo-1469474968028-56623f02e42e?auto=format&fit=crop&w=600&q=80"
            };

            return View(viewModel);
        }

        // GET: /Booking/History
        public async Task<IActionResult> History()
        {
            var history = await _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.Payments)
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
                    CustomerName = b.CustomerName,
                    Email = b.Email,
                    PhoneNumber = b.PhoneNumber,
                    PaymentStatus = b.Payments.FirstOrDefault() != null ? 
                        b.Payments.FirstOrDefault()!.PaymentStatus : "Not Paid",
                    Amount = b.Payments.FirstOrDefault() != null ? 
                        b.Payments.FirstOrDefault()!.Amount : 0,
                    ImageUrl = b.Package.ImageUrl ?? "https://images.unsplash.com/photo-1469474968028-56623f02e42e?auto=format&fit=crop&w=600&q=80"
                })
                .ToListAsync();

            return View(history);
        }

        // GET: /Booking/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            // Get bookings with package information and images
            var myBookings = await _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.Payments)
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
                    CustomerName = b.CustomerName,
                    Email = b.Email,
                    PhoneNumber = b.PhoneNumber,
                    PaymentStatus = b.Payments.FirstOrDefault() != null ? 
                        b.Payments.FirstOrDefault()!.PaymentStatus : "Not Paid",
                    Amount = b.Payments.FirstOrDefault() != null ? 
                        b.Payments.FirstOrDefault()!.Amount : 0,
                    ImageUrl = b.Package.ImageUrl ?? "https://images.unsplash.com/photo-1469474968028-56623f02e42e?auto=format&fit=crop&w=600&q=80"
                })
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(myBookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Package)
                .FirstOrDefaultAsync(b => b.BookingId == id);
            
            if (booking == null) 
                return NotFound();

            if (booking.Status == "Cancelled")
            {
                TempData["Error"] = "Booking is already cancelled.";
                return RedirectToAction("MyBookings");
            }

            booking.Status = "Cancelled";

            // Restore available seats
            booking.Package.AvailableSeats += booking.NumberOfSeats;

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == booking.BookingId);
            
            if (payment != null && payment.PaymentStatus == "Success")
            {
                var refundAmount = payment.Amount * 0.85m; // 15% deduction
                payment.PaymentStatus = "Refunded";
                payment.RefundAmount = refundAmount;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking cancelled successfully. Refund processed with 15% deduction.";
            return RedirectToAction("MyBookings");
        }

        // GET: Booking/Test - Simple test method to verify database connection
        public async Task<IActionResult> Test()
        {
            try
            {
                var packageCount = await _context.Packages.CountAsync();
                var bookingCount = await _context.Bookings.CountAsync();
                
                ViewBag.Message = $"Database connection successful! Packages: {packageCount}, Bookings: {bookingCount}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Database error: {ex.Message}";
                return View();
            }
        }
    }
}
