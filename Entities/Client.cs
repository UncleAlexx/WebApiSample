using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.Entities;

[Table(nameof(Client))]
public sealed class Client : EntityBase
{
    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Email)]
    public string Email { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Number)]
    public string WorkNumber { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Number)]
    public string AdditionalPhone { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Email)]
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