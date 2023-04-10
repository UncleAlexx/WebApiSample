using Microsoft.EntityFrameworkCore;
using EfCoreSample.Repositories;
namespace EfCoreSample.Repositories;

public interface IRepository<T2> where T2 : class
{
    dynamic? GetTable<T>() where T : T2;

    public bool Migrated { get; }

    ValueTask<T?> GetEntityById<T>(int id, CancellationToken token = default) where T : T2;

    IOrderedEnumerable<T> GetSortedById<T>(in SortOrder order) where T : T2;

    public ValueTask<bool> TryDeleteEntityById<T>(int id, DbSet<T> table, CancellationToken token = default) where T : class, T2;

    public ValueTask<bool> TryAddEntities<T>(DbSet<T> table, params T[] entities) where T : class, T2;

    public ValueTask<bool> TryDeleteEntitiesByIds<T>(DbSet<T> table, params int[] ids) where T : class, T2;

    public ValueTask<bool> TryUpdateEntities<T>(DbSet<T> table, params T[] values) where T : class, T2;
}
