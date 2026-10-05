namespace SweetFactory.Models;

public class User
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<WorkEntry> WorkEntries { get; set; } = [];
}
