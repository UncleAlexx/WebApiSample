using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using EfCoreSample.Entities;
using EfCoreSample.DatabaseContext;
using EfCoreSample.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using FluentValidation;
using EfCoreSample.Validation;
using System.Reflection;

AutoserviceContext dbContext = new();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AutoserviceContext>();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation
    (fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
var provider = builder.Services.BuildServiceProvider();
var autoserviceRepo = await AutoserviceRepositoryFactory.CreateAutoserviceRepository((AutoserviceContext)provider.GetService(typeof(AutoserviceContext))!);
builder.Services.AddTransient(async x => await AutoserviceRepositoryFactory.CreateAutoserviceRepository((AutoserviceContext)provider.GetService(typeof(DbContext))!));
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/Providers",
    [ProducesResponseType(statusCode: 200, type: typeof(Provider))]
() => Results.Ok(autoserviceRepo.GetTable<Provider>()));

app.MapGet("/ProviderById{id}",
    [ProducesResponseType(statusCode: 200, type: typeof(Provider))]
async (int id) =>
    {
        Provider? entityById = await autoserviceRepo.GetEntityById<Provider>(id);
        return entityById is null ? 
        Results.BadRequest($"Provider with id {id} not found in table {nameof(Provider)}") 
            : Results.Ok(entityById);
    });

app.MapGet("/SortProviderByDescendingById{order}",
    [ProducesResponseType(statusCode: 200, type: typeof(Provider))]
(SortOrder order) =>
    {
        var orderedProvidersByDescending = autoserviceRepo.GetSortedById<Provider>(order);
        return orderedProvidersByDescending is null ? Results.BadRequest($"incorrect Sort Order {(int)order} table {nameof(Provider)}") :
            Results.Ok(orderedProvidersByDescending);
    });

app.MapPut("/Update{provider}",
    [ProducesResponseType(statusCode: 200, type: typeof(Provider))]
async Task<IResult> (Provider provider, AutoserviceContext autoservice, IValidator<Provider> validator) =>
    {
        List<string> requiredErrors = new(20);

        var validationResult = validator.Validate(provider);

        var nonRequiredErrors = provider.GetNonRequiredErrors();
        if (validationResult.IsValid & nonRequiredErrors.First().IsValid)
        {
            if (autoservice.Providers.Any(x => x.Id == provider.Id) is false)
            {
                return Results.BadRequest($"updating failed an id doesnt exist in table {nameof(Provider)}");
            }

            try
            {
                autoservice.Providers.Update(provider);
                await autoservice.SaveChangesAsync();
                return Results.Created("/UpdateProviderById/{provider})", provider);
            }
            catch
            {
                return Results.BadRequest($"updating failed in table {nameof(Provider)}");
            }
        }
        requiredErrors.AddRange(validationResult.Errors.Select(x => $"errorType = required, message = {x.ErrorMessage}"));
        requiredErrors.AddRange(nonRequiredErrors.Where(x => x.IsValid is false).Select(x => 
            $"error type is {nameof(NonRequiredMemberError).ToLower().AsSpan()[..11]} error is {x.Error.Message}"));
        return Results.BadRequest(requiredErrors);
    });

app.MapDelete("/DeleteProviderByIds{ids}",
    [ProducesResponseType(statusCode: 200, type: typeof(Provider))]
async (int[] ids, AutoserviceContext autoservice) =>
    {
        if (ids.Any(id => autoservice.Providers.Any(provider => provider.Id == id) is false))
        {
            return Results.BadRequest($"wasn't deleted an id doesnt exist in {nameof(Provider)} table");
        }
        try
        {
            autoservice.Providers.RemoveRange(ids.Select(id => autoservice.Providers.First((x => x.Id == id))));
            await autoservice.SaveChangesAsync();
        }
        catch (Exception d)
        {
            Debug.WriteLine(d);
            return Results.BadRequest("wasn't deleted error occured");
        }
        return Results.Accepted("/DeletedProviderByIds{ids}");

    });

app.MapPut("/Add{provider}",
    [ProducesResponseType(statusCode: 200, type: typeof(Provider))]
async Task<IResult> (Provider provider, AutoserviceContext autoservice, IValidator<Provider> validator) =>
    {
        List<string> requiredErrors = new(20);
        var nonRequiredErrors = provider.GetNonRequiredErrors();
        var validationResult = validator.Validate(provider);
        if (validationResult.IsValid && nonRequiredErrors.First().IsValid)
        {
            try
            {
                autoservice.Providers.Add(provider);
                await autoservice.SaveChangesAsync();
                return Results.Created("/Add/{provider}", provider);
            }
            catch
            {
                return Results.BadRequest("adding failed");
            }
        }
        requiredErrors.AddRange(validationResult.Errors.Select(x => $"errorType = required, message = {x.ErrorMessage}"));

        requiredErrors.AddRange(nonRequiredErrors.Where(x => x.IsValid is false).Select(x => $"error type is " +
            $"{nameof(NonRequiredMemberError).ToLower().AsSpan()[..11]} error is {x.Error.Message}"));
        return Results.BadRequest(requiredErrors);
    });
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
