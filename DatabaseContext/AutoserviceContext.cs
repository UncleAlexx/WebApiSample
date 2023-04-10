using EfCoreSample.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.DatabaseContext;

public sealed class AutoserviceContext : DbContext
{
    private readonly bool _migrated = false;

    public bool Migrated => _migrated;
    [AllowNull]
    public DbSet<Parts> Parts { get; set; }

    [AllowNull]
    public DbSet<Employee> Employees { get; set; }

    [AllowNull]
    public DbSet<Car> Cars { get; set; }

    [AllowNull]
    public DbSet<Client> Clients { get; set; }

    [AllowNull]
    public DbSet<Provider> Providers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlServer($@"Server = desktop\SQLEXPRESS; Database=Autoservice4;Trusted_Connection=True;
            MultipleActiveResultSets=true;TrustServerCertificate=true");
}
