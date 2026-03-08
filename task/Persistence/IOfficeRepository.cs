using task.Models.Entities;

namespace task.Persistence;

public interface IOfficeRepository
{
    Task<int> GetOfficeCountAsync(CancellationToken token);

    Task ReplaceAllAsync(IReadOnlyCollection<Office> offices, IReadOnlyCollection<Phone> phones, CancellationToken token);
}