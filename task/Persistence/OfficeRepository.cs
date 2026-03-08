using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using task.Models.Entities;

namespace task.Persistence;

public class OfficeRepository : IOfficeRepository
{
    private readonly AppDbContext _context;

    public OfficeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetOfficeCountAsync(CancellationToken token) =>
        await _context.Offices.CountAsync(token);

    public async Task ClearAsync(CancellationToken token)
    {
        await _context.Phones.ExecuteDeleteAsync(token);
        await _context.Offices.ExecuteDeleteAsync(token);
    }

    public async Task ReplaceAllAsync(IReadOnlyCollection<Office> offices, IReadOnlyCollection<Phone> phones, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);

        await _context.Phones.ExecuteDeleteAsync(token);
        await _context.Offices.ExecuteDeleteAsync(token);

        var bulkConfig = new BulkConfig { PreserveInsertOrder = true, BatchSize = 5000 };

        await _context.BulkInsertAsync(offices.ToList(), bulkConfig, cancellationToken: token);
        await _context.BulkInsertAsync(phones.ToList(), bulkConfig, cancellationToken: token);

        await transaction.CommitAsync(token);
    }
}