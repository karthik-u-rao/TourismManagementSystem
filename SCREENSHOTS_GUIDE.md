# Screenshots Guide for Tourism Management System

This guide helps you capture the necessary screenshots for the README.md file.

## Directory Structure

Create the following directory structure in your project root:

```
screenshots/
??? homepage.png              # Landing page with hero section
??? packages.png              # Package listing page
??? package-details.png       # Package details view
??? booking-form.png          # Booking creation form
??? booking-confirmation.png  # Booking confirmation page
??? admin-dashboard.png       # Admin dashboard with charts
??? user-management.png       # Admin user management
??? payment.png              # Payment checkout page
??? admin-packages.png        # Admin package management
??? mobile-view.png           # Mobile responsive view
```

## Screenshot Checklist

### 1. Homepage (homepage.png)
- Hero section with search form
- Featured packages section
- Features section
- Navigation bar visible
- URL: https://localhost:5001/

### 2. Package Listing (packages.png)
- All packages displayed in grid
- Search/filter functionality
- Package cards with images
- Pricing and availability info
- URL: https://localhost:5001/Package

### 3. Package Details (package-details.png)
- Package hero image
- Detailed information
- Booking sidebar
- Inclusions/exclusions
- URL: https://localhost:5001/Package/Details/1

### 4. Booking Form (booking-form.png)
- Customer information form
- Package summary sidebar
- Validation messages
- Total calculation
- URL: https://localhost:5001/Booking/Create/1

### 5. Booking Confirmation (booking-confirmation.png)
- Booking details
- Confirmation number
- Payment information
- Next steps
- URL: https://localhost:5001/Booking/Confirmation/1

### 6. Admin Dashboard (admin-dashboard.png)
- Statistics cards
- Charts and graphs
- Recent bookings table
- Quick actions
- URL: https://localhost:5001/Admin/Dashboard
- Login: karthik@tourism.com / Karthik@123

### 7. User Management (user-management.png)
- User list table
- User actions
- Role information
- Admin navigation
- URL: https://localhost:5001/Admin/Users

### 8. Payment Process (payment.png)
- Payment form
- Booking summary
- Security information
- Total amount
- URL: https://localhost:5001/Payment/Checkout/1

### 9. Admin Package Management (admin-packages.png)
- Package grid view
- CRUD action buttons
- Package statistics
- Add new package button
- URL: https://localhost:5001/Admin/Packages

### 10. Mobile View (mobile-view.png)
- Responsive navigation
- Mobile-optimized forms
- Touch-friendly buttons
- Readable text sizes
- Use browser dev tools to simulate mobile

## Screenshot Best Practices

### Browser Settings
- Use Chrome or Edge for consistent rendering
- Set browser zoom to 100%
- Use 1920x1080 resolution for desktop screenshots
- Use 375x812 (iPhone X) for mobile screenshots

### Content Preparation
1. Seed sample data before taking screenshots
2. Login as admin for admin screenshots
3. Clear browser cache for clean loading
4. Hide personal information if any

### Image Quality
- Save as PNG format for crisp quality
- Use high resolution (at least 1200px width)
- Ensure good contrast and readability
- Crop appropriately to focus on relevant content

## Sample Data for Screenshots

### Admin User
- Email: karthik@tourism.com
- Password: Karthik@123

## Mobile Screenshots

For mobile screenshots, use browser dev tools:

1. Open Chrome DevTools (F12)
2. Click device toggle (Ctrl+Shift+M)
3. Select "iPhone X" or "Pixel 2"
4. Take screenshots of key pages

## Image Optimization

After taking screenshots:

1. Resize if larger than 2MB
2. Optimize using tools like TinyPNG
3. Rename according to the naming convention
4. Verify images load properly in README

## Quality Checklist

Before finalizing screenshots:

- [ ] All text is readable
- [ ] UI elements are clearly visible
- [ ] No personal/sensitive information
- [ ] Consistent browser/theme
- [ ] Proper aspect ratios
- [ ] No loading spinners/incomplete renders
- [ ] Representative data shown
- [ ] Professional appearance

## Screenshot Descriptions

When updating README.md, use descriptive alt text:

```markdown
![Homepage](screenshots/homepage.png)
*Modern landing page with hero section and featured packages*

![Admin Dashboard](screenshots/admin-dashboard.png)
*Comprehensive admin dashboard with analytics and charts*
```

Remember: Good screenshots make your project more appealing to potential users and contributors.