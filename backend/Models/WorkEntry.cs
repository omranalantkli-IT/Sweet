namespace SweetFactory.Models;

public class WorkEntry
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public User Worker { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public DateOnly WorkDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitRateSnapshot { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount => Quantity * UnitRateSnapshot;
}
