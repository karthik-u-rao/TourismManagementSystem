# Contributing to Tourism Management System

Thank you for your interest in contributing to the Tourism Management System. We welcome contributions from the community.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How to Contribute](#how-to-contribute)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Pull Request Process](#pull-request-process)
- [Issue Reporting](#issue-reporting)

## Code of Conduct

This project is governed by a Code of Conduct. By participating, you are expected to uphold this code.

### Our Standards

- Use welcoming and inclusive language
- Be respectful of differing viewpoints and experiences
- Gracefully accept constructive criticism
- Focus on what is best for the community
- Show empathy towards other community members

## How to Contribute

### Ways to Contribute

1. Bug Reports - Report bugs through GitHub issues
2. Feature Requests - Suggest new features or improvements
3. Code Contributions - Submit bug fixes or new features
4. Documentation - Improve or add documentation
5. Testing - Help test new features and report issues

### Before You Start

1. Check existing issues to avoid duplicates
2. For major features, create an issue first to discuss
3. Fork the repository and create a feature branch
4. Follow our coding standards and guidelines

## Development Setup

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB is fine)
- Visual Studio 2022 or VS Code
- Git

### Setup Steps

```bash
# 1. Fork and clone the repository
git clone https://github.com/YOUR-USERNAME/TourismManagementSystem.git
cd TourismManagementSystem

# 2. Create a new branch
git checkout -b feature/your-feature-name

# 3. Restore packages
dotnet restore

# 4. Update database
dotnet ef database update --project Tourism.DataAccess --startup-project TourismManagementSystem

# 5. Run the application
dotnet run --project TourismManagementSystem
```

## Coding Standards

### C# Guidelines

- Follow Microsoft C# coding conventions
- Use meaningful names for variables, methods, and classes
- Add XML documentation for public APIs
- Keep methods small and focused
- Use SOLID principles

### Example Code Style

```csharp
/// <summary>
/// Validates a package for booking availability
/// </summary>
/// <param name="package">The package to validate</param>
/// <returns>True if package is available for booking</returns>
public bool IsPackageAvailableForBooking(Package package)
{
    if (package == null)
        throw new ArgumentNullException(nameof(package));

    return package.AvailableSeats > 0 && 
           package.StartDate > DateTime.Today &&
           package.EndDate > package.StartDate;
}
```

### Frontend Guidelines

- Use Bootstrap classes for styling
- Keep JavaScript functions small and focused
- Add comments for complex logic
- Use semantic HTML elements
- Ensure accessibility compliance

### Database Guidelines

- Use Entity Framework conventions
- Add proper indexes for performance
- Use meaningful table and column names
- Add appropriate constraints

## Pull Request Process

### Before Submitting

1. Ensure your code builds without warnings
2. Run existing tests and ensure they pass
3. Add tests for new functionality
4. Update documentation if needed
5. Follow the coding standards
6. Squash commits if necessary

### PR Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Performance improvement
- [ ] Refactoring

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] Tests added/updated
```

### Review Process

1. Automated checks must pass
2. At least one maintainer review required
3. All conversations must be resolved
4. Final approval from project maintainer

## Issue Reporting

### Bug Reports

Use the bug report template and include:

- Environment: OS, .NET version, browser
- Steps to Reproduce: Clear, numbered steps
- Expected Behavior: What should happen
- Actual Behavior: What actually happens
- Screenshots: If applicable
- Additional Context: Any other relevant information

### Bug Report Template

```markdown
**Environment**
- OS: Windows 11
- .NET Version: 8.0
- Browser: Chrome 90+

**Steps to Reproduce**
1. Navigate to...
2. Click on...
3. Enter...
4. See error

**Expected Behavior**
A clear description of what you expected to happen.

**Actual Behavior**
A clear description of what actually happened.

**Screenshots**
If applicable, add screenshots to help explain the problem.

**Additional Context**
Add any other context about the problem here.
```

## Testing Guidelines

### Unit Tests

```csharp
[Test]
public void IsPackageAvailableForBooking_WithValidPackage_ReturnsTrue()
{
    // Arrange
    var package = new Package
    {
        AvailableSeats = 10,
        StartDate = DateTime.Today.AddDays(7),
        EndDate = DateTime.Today.AddDays(14)
    };
    var service = new PackageValidationService();

    // Act
    var result = service.IsPackageAvailableForBooking(package);

    // Assert
    Assert.IsTrue(result);
}
```

## Project Structure

Understanding the project structure helps with contributions:

```
TourismManagementSystem/
??? Controllers/           # MVC Controllers
??? Views/                # Razor Views
??? Services/             # Business Logic
??? Models/              # View Models
??? Areas/Identity/      # Authentication
??? wwwroot/            # Static Files

Tourism.DataAccess/
??? Models/             # Entity Models
??? Migrations/         # EF Migrations
??? TourismDbContext.cs # Database Context
```

## Code Review Guidelines

### For Reviewers

- Be constructive and helpful
- Explain the reasoning behind suggestions
- Approve when code meets standards
- Test the changes locally if possible

### For Contributors

- Respond to feedback promptly
- Ask questions if feedback is unclear
- Make requested changes or explain why not
- Be patient during the review process

## Resources

### Documentation

- ASP.NET Core Documentation
- Entity Framework Core
- Bootstrap Documentation

### Tools

- Visual Studio 2022
- SQL Server Management Studio
- Git

## Getting Help

- Email: Contact through GitHub issues
- Issues: Create a GitHub issue
- Documentation: Check the project README

Thank you for contributing to Tourism Management System