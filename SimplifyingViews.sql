-- Create a simple view that hides the complexity
CREATE VIEW SimpleUsers AS
SELECT 
    Id,
    Email,
    FullName,
    UserName
FROM AspNetUsers;

-- Create a view for bookings with user info
CREATE VIEW BookingDetails AS
SELECT 
    b.BookingId,
    b.BookingDate,
    b.CustomerName,
    b.Email,
    b.NumberOfSeats,
    b.Status,
    p.Name as PackageName,
    p.Location,
    p.Price
FROM Bookings b
JOIN Packages p ON b.PackageId = p.PackageId;

-- Now you can use simple queries:
SELECT * FROM SimpleUsers;
SELECT * FROM BookingDetails;