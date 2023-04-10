using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EfCoreSample.DatabaseContext;
public static class AutoserviceContextExtensions
{
    private const string _migratedPrivatePropertyName = "_migrated";
    private static readonly PropertyInfo? _migratedInfo = typeof(AutoserviceContext).GetProperty(_migratedPrivatePropertyName, BindingFlags.NonPublic | BindingFlags.Instance);

    public static async Task MigrateIfRequired(this AutoserviceContext context, TimeSpan timeout = default)
    {
        if (timeout == default)
            timeout = Timeout.InfiniteTimeSpan;
        ArgumentNullException.ThrowIfNull(context, (nameof(context)));

        using (var migrationTimeout = new CancellationTokenSource(timeout))
        {
            if (await IsMigrationNeeded(context!, migrationTimeout.Token))
            {
                if (migrationTimeout.TryReset() is false)
                {
                    using (var operationCancellation = new CancellationTokenSource(timeout))
                    {
                        await context!.Database.MigrateAsync(operationCancellation.Token);
                        _migratedInfo.SetValue(context, true);
                        SetMigratedInfo(true, _migratedInfo!, context);
                    }
                    return;
                }
                await context!.Database.MigrateAsync(migrationTimeout.Token);

                SetMigratedInfo(true, _migratedInfo!, context);
            }
        }
    }

    private static void SetMigratedInfo(bool value, in PropertyInfo property, in AutoserviceContext context) 
    {
        if ((bool)property!.GetValue(context)! is false)
            _migratedInfo?.SetValue(context, value);
    }

    private static async ValueTask<bool> IsMigrationNeeded(DbContext context, CancellationToken token) =>
        (await context!.Database.GetPendingMigrationsAsync(token)).Any();
}
