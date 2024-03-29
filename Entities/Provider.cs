﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EfCoreSample.Entities;

[Table(nameof(Provider))]
public sealed class Provider : EntityBase
{
    [MaybeNull]
    public string Company { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Number)]
    public string WorkPhone { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Number)]
    public string AdditionalPhone { get; set; }

    [MaybeNull, Validation.NonRequiredMembersValidation(Validation.ValidationType.Email)]
    public string Email { get; set; }

    [MaybeNull, ForeignKey("ProviderId")]
    public ICollection<Parts> Parts { get; set; }

    [MaybeNull, ForeignKey("ProviderId")]
    public ICollection<Car> Cars { get; set; }

    [Required]
    public required string? Address { get; set; }

    [Required]
    public required string? Phone { get; set; }

    [Required]
    public required string? FullName { get; set; }
}
