using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.Entities;

[Table(nameof(Client))]
public sealed class Client : EntityBase
{
    [MaybeNull]
    public string Email { get; set; }

    [MaybeNull]
    public string WorkNumber { get; set; }

    [MaybeNull]
    public string AdditionalPhone { get; set; }

    [MaybeNull]
    public string AdditionalEmail { get; set; }

    [MaybeNull]
    public Car Car { get; set; }

    [MaybeNull]
    public Employee Employee { get; set; }

    [Column("CarID")]
    public required int CarId { get; set; }

    [Column("EmployeeID"), Required]
    public required int EmployeeId { get; set; }

    [Required]
    public required string? FullName { get; set; }

    [Required]
    public required string? Address { get; set; }

    [Required]
    public required string? Phone { get; set; }
}