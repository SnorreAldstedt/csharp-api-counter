using api_counter.wwwapi9.Data;
using api_counter.wwwapi9.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Demo API");
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

CounterHelper.Initialize();

var counters = app.MapGroup("/counters");
//TODO: 1. write a method that returns all counters in the counters list.  use method below as a starting point
counters.MapGet("/", () =>
{
    return Results.Ok(CounterHelper.GetAllCounters());
});


//TODO: 2. write a method to return a single counter based on the id being passed in.  complete method below
//[ProcedureResponseType(StatusCodes.Status404NotFound)]
counters.MapGet("/{id}", (int id) =>
{
    bool hasId = CounterHelper.Counters.Any(c => c.Id == id);
    if (!hasId)
    {
        return Results.NotFound();
    }
    Counter idCounter = CounterHelper.Counters.First(c => c.Id == id);
    return Results.Ok(idCounter);
// Counter? idCounter = CounterHelper.GetCounter(id);
});

//TODO: 3.  write another method that returns counters that have a value greater than the {number} passed in.        
counters.MapGet("/greaterthan/{number}", (int number) =>
{
    return Results.Ok(CounterHelper.GetAllGreaterCounters(number));
});

counters.MapGet("/lessthan/{number}", (int number) =>
{
    return Results.Ok(CounterHelper.GetAllSmallerCounters(number));
});

counters.MapPut("/increment/{id}", (int id) =>
{
    bool hasId = CounterHelper.Counters.Any(c => c.Id == id);
    if (!hasId)
    {
        return Results.NotFound();
    }
    Counter idCounter = CounterHelper.Counters.First(c => c.Id == id);
    idCounter.Value++;
    return Results.Ok(idCounter);

});

counters.MapPut("/decrease/{id}", (int id) =>
{
    bool hasId = CounterHelper.Counters.Any(c => c.Id == id);
    if (!hasId)
    {
        return Results.NotFound();
    }
    Counter idCounter = CounterHelper.Counters.First(c => c.Id == id);
    idCounter.Value--;
    return Results.Ok(idCounter);

});
////TODO:4. write another method that returns counters that have a value less than the {number} passed in.

//Extension #1
//TODO:  1. Write a controller method that increments the Value property of a counter of any given Id.
//e.g.  with an Id=1  the Books counter Value should be increased from 5 to 6
//return the counter you have increased

//Extension #2
//TODO: 2. Write a controller method that decrements the Value property of a counter of any given Id.
//e.g.  with an Id=1  the Books counter Value should be decreased from 5 to 4
//return the counter you have decreased


//Super Optional Extension #1 - Refactor the code!
// Refactor - move the EndPoints into their own class and ensure they are mapped correctly
// Refactor - add a repository layer: interface & concrete class, inject this into the endpoint using the builder.Service

//Super Optional Extension #2
//Create a new endpoint that passes a json collection of strings to a controller method and adds them as new Counters with an appropriate unique integer id AND a value of 0.





app.Run();
