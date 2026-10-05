using System.ComponentModel.DataAnnotations;

namespace SweetFactory.Dtos;

public record LoginRequest([Required] string Username, [Required] string Password);
public record AuthResponse(string Token, UserDto User);
public record UserDto(int Id, string FullName, string Username, string Role, bool IsActive);
public record CreateWorkerRequest([Required, MaxLength(100)] string FullName,
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, MinLength(8)] string Password);
public record UpdateWorkerRequest([Required, MaxLength(100)] string FullName, bool IsActive, string? NewPassword);
public record ProductDto(int Id, string Name, string UnitName, decimal RatePerUnit, bool IsActive);
public record SaveProductRequest([Required, MaxLength(100)] string Name,
    [Required, MaxLength(30)] string UnitName, [Range(0, 1_000_000)] decimal RatePerUnit, bool IsActive = true);
public record CreateWorkEntryRequest([Range(1, int.MaxValue)] int ProductId,
    DateOnly WorkDate, [Range(0.01, 1_000_000)] decimal Quantity, [MaxLength(500)] string? Notes);
public record WorkEntryDto(int Id, int WorkerId, string WorkerName, int ProductId, string ProductName,
    string UnitName, DateOnly WorkDate, decimal Quantity, decimal UnitRate, decimal TotalAmount, string? Notes);
public record MonthlyWorkerSummary(int WorkerId, string WorkerName, decimal TotalQuantity, decimal TotalAmount);
public record AdminDashboardDto(int ActiveWorkers, decimal TodayQuantity, decimal MonthTotal,
    int MonthEntries, IReadOnlyList<WorkEntryDto> RecentEntries);
