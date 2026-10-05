using Microsoft.EntityFrameworkCore;
using SweetFactory.Application.Contracts;
using SweetFactory.Domain.Entities;
using SweetFactory.Infrastructure.Persistence;

namespace SweetFactory.Application.Services;

public class WorkEntryService(AppDbContext db)
{
    public async Task<WorkEntryDto?> CreateAsync(int workerId, string workerName, CreateWorkEntryRequest request)
    {
        var product = await db.Products.FindAsync(request.ProductId);
        if (product is null || !product.IsActive) return null;
        var entry = new WorkEntry
        {
            WorkerId = workerId,
            ProductId = product.Id,
            WorkDate = request.WorkDate,
            Quantity = request.Quantity,
            UnitRateSnapshot = product.RatePerUnit,
            Notes = request.Notes?.Trim()
        };
        db.WorkEntries.Add(entry);
        await db.SaveChangesAsync();
        return new WorkEntryDto(entry.Id, workerId, workerName, product.Id, product.Name,
            product.UnitName, entry.WorkDate, entry.Quantity, entry.UnitRateSnapshot, entry.TotalAmount, entry.Notes);
    }

    public async Task<IReadOnlyList<WorkEntryDto>> GetAsync(int currentId, bool isAdmin,
        int? workerId, DateOnly? from, DateOnly? to)
    {
        var query = db.WorkEntries.AsNoTracking().AsQueryable();
        if (!isAdmin) query = query.Where(entry => entry.WorkerId == currentId);
        else if (workerId.HasValue) query = query.Where(entry => entry.WorkerId == workerId.Value);
        if (from.HasValue) query = query.Where(entry => entry.WorkDate >= from.Value);
        if (to.HasValue) query = query.Where(entry => entry.WorkDate <= to.Value);
        return await query.OrderByDescending(entry => entry.WorkDate).ThenByDescending(entry => entry.Id)
            .Select(entry => new WorkEntryDto(entry.Id, entry.WorkerId, entry.Worker.FullName,
                entry.ProductId, entry.Product.Name, entry.Product.UnitName, entry.WorkDate,
                entry.Quantity, entry.UnitRateSnapshot, entry.Quantity * entry.UnitRateSnapshot, entry.Notes))
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entry = await db.WorkEntries.FindAsync(id);
        if (entry is null) return false;
        db.WorkEntries.Remove(entry);
        await db.SaveChangesAsync();
        return true;
    }
}
