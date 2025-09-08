// Helper service to simplify user operations
public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    // Simple method - hides the complexity
    public async Task<List<SimpleUser>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(u => new SimpleUser 
        {
            Email = u.Email,
            FullName = u.FullName,
            Id = u.Id
        }).ToList();
    }
    
    // Simple user data class
    public class SimpleUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}