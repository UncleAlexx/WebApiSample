using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EfCoreSample.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace EfCoreSample.Validation;

public static class EntityExtensions
{
    public static IEnumerable<(NonRequiredMemberError Error, bool IsValid)> GetAllNonRequiredErrors<T>(this IEnumerable<T> entities) where T : EntityBase => 
        entities.Select(entity => entity.GetNonRequiredErrors()).SelectMany(errors => errors);
    
    public static IEnumerable<(NonRequiredMemberError Error, bool IsValid)> GetNonRequiredErrors<T>(this T entity) where T : EntityBase
    {
        if (entity is null)
        {
            yield return (new NonRequiredMemberError("nullreference", $"table {typeof(T).Name} doesnt exist"), false);
            yield break;
        }
        var type = entity!.GetType();
        var nonRequiredProps = type.GetProperties().
            Where(x => x.IsDefined(typeof(NonRequiredMembersValidationAttribute), false) && x.IsDefined(typeof(RequiredMemberAttribute), false) is false);

        if (nonRequiredProps.Any() is false)
        {
            yield return (new NonRequiredMemberError("entity hasn't non-required props to validate",
                $"table {typeof(T).Name} doesn't have non required emails or phones record id is {entity.Id}"), false);
            yield break;
        }

        if(nonRequiredProps.All(prop => prop.GetValue(entity) == null))
        {
            yield return (null, true)!;
            yield break;
        }

        bool anyErrors = false;
        foreach (var propInfo in nonRequiredProps)
        {
            var value = propInfo.GetValue(entity);
            if (value is null)
            {
                continue;
            }
            var validationType = propInfo!.GetCustomAttribute<NonRequiredMembersValidationAttribute>()!.Type;
            if (IsValid(validationType, value as string) is false)
            {   
                anyErrors = true;
                yield return (new NonRequiredMemberError($"{validationType} validation", 
                    $"value {value} of field {propInfo.Name} doesn't correspond to pattern, pattern type {validationType} " +
                    $"in table {type.Name} entity id is {entity.Id}"), false);
            }
            if(!anyErrors)
                yield return (null, true);
        }
    }

    private static bool IsValid(in ValidationType validationType, [AllowNull] in string value)
    {
        return validationType switch
        {
            ValidationType.Email => ValidationPatterns.EmailPattern().IsMatch(value!),
            ValidationType.Number => ValidationPatterns.PhonePattern().IsMatch(value!),
            _ => throw new ArgumentException($"Unhandled Validation type argname {nameof(validationType)} value is {(int)validationType}")
        };
    }
}

