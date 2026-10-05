namespace SweetFactory.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string UnitName { get; set; }
    public decimal RatePerUnit { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<WorkEntry> WorkEntries { get; set; } = [];
}
