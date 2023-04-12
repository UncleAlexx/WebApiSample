using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.Entities;

[Table(nameof(Parts))]
public sealed class Parts : EntityBase
{
    [MaybeNull]
    public string Brand { get; set; }

    [Column("ProviderID")]
    public required int ProviderId { get; set; }

    [Required]
    public required string? Type { get; set; }

    [Required]
    public required int Count { get; set; }

    [MaybeNull]
    public Provider Provider { get; set; }
}
