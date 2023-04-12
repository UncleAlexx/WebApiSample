using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.Entities;

[Table(nameof(Car))]
public sealed class Car : EntityBase
{
    [MaybeNull, ForeignKey("CarId")]
    public ICollection<Client> Clients { get; set; }

    [MaybeNull]
    public Provider Provider { get; set; }

    [Column("ProviderID"), Required]
    public required int ProviderId { get; set; }

    [Required]
    public required string? Brand { get; set; }

    [Required]
    public required string? Color { get; set; }

    [Required]
    public required string? Type { get; set; }

    [Required]
    public required string? Tires { get; set; }

    [Required]
    public required double Weight { get; set; }

    public double Mileage { get; set; }
}
