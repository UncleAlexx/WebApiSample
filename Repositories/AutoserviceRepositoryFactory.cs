using EfCoreSample.DatabaseContext;
using EfCoreSample.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace EfCoreSample.Repositories;

public static class AutoserviceRepositoryFactory
{
    private sealed class AutoserviceRepository : IRepository<EntityBase> 
    { 
        private readonly AutoserviceContext _dbContext;

        private static readonly Func<AutoserviceContext, IAsyncEnumerable<Client>> _clients = EF.CompileAsyncQuery((AutoserviceContext context) => context.Clients);

        private static readonly Func<AutoserviceContext, IAsyncEnumerable<Parts>> _parts = EF.CompileAsyncQuery((AutoserviceContext context) => context.Parts);

        private static readonly Func<AutoserviceContext, IAsyncEnumerable<Employee>> _employees = EF.CompileAsyncQuery((AutoserviceContext context) => context.Employees);

        private static readonly Func<AutoserviceContext, IAsyncEnumerable<Car>> _cars = EF.CompileAsyncQuery((AutoserviceContext context) => context.Cars);

        private static readonly Func<AutoserviceContext, IAsyncEnumerable<Provider>> _providers = EF.CompileAsyncQuery((AutoserviceContext context) => context.Providers);

        public AutoserviceRepository(in AutoserviceContext context) => _dbContext = context;

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IAsyncEnumerable? of T<T> where T : EntityBase</T></returns>
        /// 
        dynamic? IRepository<EntityBase>.GetTable<T>()
        {
            return typeof(T) switch
            {
                var type when type == typeof(Parts) => _parts(_dbContext),
                var type when type == typeof(Client) => _clients(_dbContext),
                var type when type == typeof(Provider) => _providers(_dbContext),
                var type when type == typeof(Car) => _cars(_dbContext),
                var type when type == typeof(Employee) => _employees(_dbContext),
                _ => null
            };
        }

        public bool Migrated => _dbContext?.Migrated??false;


        async ValueTask<(T[] added, bool isCompleted)> IRepository<EntityBase>.TryAddEntities<T>(DbSet<T> table, params T[] entities)
        {
            try
            {
                if(entities.Any(x => table.Any(entity =>  entity.Id == x.Id))) 
                {
                    Trace.WriteLine("an entity with an id already exists in the table");
                    return (Array.Empty<T>(), false);
                }
                table!.AddRange(entities);
                await _dbContext.SaveChangesAsync();
                return (entities, true);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return (Array.Empty<T>(), false);
            }
        }

        public async ValueTask<T?> GetEntityById<T>(int id, CancellationToken token) where T : EntityBase
        {
            IAsyncEnumerable<T>? table = (this as IRepository<EntityBase>).GetTable<T>();

            await foreach (EntityBase? row in table!.WithCancellation(token))
            {
                if (row.Id == id)
                {
                    return (T)row;
                }
            }
            return default;
        }

        IOrderedEnumerable<T> IRepository<EntityBase>.GetSortedById<T>(in SortOrder order)
        {
            IAsyncEnumerable<T>? table = (this as IRepository<EntityBase>).GetTable<T>();
            return order switch
            {
                SortOrder.Ascending => table!.ToBlockingEnumerable()!.OrderBy(x => x.Id),
                SortOrder.Descending => table!.ToBlockingEnumerable()!.OrderByDescending(x => x.Id),
                _ => null!
            };
        }

        async ValueTask<(T deleted, bool isCompleted)> IRepository<EntityBase>.TryDeleteEntityById<T>(int id, DbSet<T> table, CancellationToken token)
        {
            try
            {
                if (table.Any(entity => entity.Id == id) is false)
                {
                    Trace.TraceError($"entity with id {id} doesn't exist and can't be deleted");
                    return (null!, false);
                }
                var entity = await (this as IRepository<EntityBase>).GetEntityById<T>(id, token) ??
                    throw new InvalidOperationException($"id doesnt exist in table {typeof(T).Name}");

                table.Remove(entity!);
                _dbContext.SaveChanges();
                return (entity, true);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                return (null!, false);  
            }
        }

        async ValueTask<(T[], bool)> IRepository<EntityBase>.TryDeleteEntitiesByIds<T>(DbSet<T> table, params int[] ids)
        {
            try
            {
                if (ids.All(id => table!.Any(x => x.Id == id)) is false)
                    throw new InvalidOperationException($"an id doesnt exist in the table {typeof(T).Name}");

                T[] toDelete = ids.Select(id => table.First(entity => entity.Id == id)).ToArray();
                table?.RemoveRange(toDelete);
                await _dbContext.SaveChangesAsync();
                return (toDelete, true);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                return (Array.Empty<T>(), false);
            }
        }

        async ValueTask<(T[] updated, bool isCompleted)> IRepository<EntityBase>.TryUpdateEntities<T>(DbSet<T> table, params T[] values)
        {
            try
            {
                if (values.Select(x => x.Id).All(id => table!.Any(x => x.Id == id)) is false)
                    throw new InvalidOperationException($"an id doesnt exist in the table {typeof(T).Name}");
                var oldEntities = values.Select(x => table.First(entity => entity.Id == x.Id));

                table.UpdateRange(values);
                await _dbContext.SaveChangesAsync();
                return (values, true);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                return (values, false);
            }
        }
    }
   
    public static async ValueTask<IRepository<EntityBase>> CreateAutoserviceRepository(AutoserviceContext context, TimeSpan timeout = default)
    { 
        IRepository<EntityBase> autoserviceRepo = new AutoserviceRepository(context);
        if (autoserviceRepo.Migrated is false)
        {
            await context.MigrateIfRequired(timeout);
        }
        return autoserviceRepo;
    }
}

public enum SortOrder
{
    Ascending,
    Descending
}