using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.Entities;

[Table(nameof(Employee))]
public sealed class Employee : EntityBase
{
    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Number)]
    public string AdditionalPhone { get; set; }

    [MaybeNull, Required]
    public required string? FullName { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Email)]
    public string Email { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Number)]
    public string WorkPhone { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Email)]
    public string AdditionalEmail { get; set; }

    [MaybeNull, ForeignKey("EmployeeId")]
    public ICollection<Client> Clients { get; set; }

    [Required]
    public required string? Post { get; set; }

    [Required]
    public required string? Address { get; set; }

    [Required]
    public required string? Phone { get; set; }

    [Column("Salary", TypeName = "decimal(18,10)")]
    public required decimal Salary { get; set; }
}
