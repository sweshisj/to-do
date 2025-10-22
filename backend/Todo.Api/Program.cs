using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Todo.api;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<ITodoStore, InMemoryTodoStore>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "To-Do API",
        Version = "v1",
        Description = "A minimal To-Do API"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints
app.MapGet("/todos",
    ([FromServices] ITodoStore store) =>
    {
        return Results.Ok(store.GetAll());
    })
    .Produces(StatusCodes.Status200OK)
    .WithName("GetAllTodos");

app.MapPost("/todos",
    ([FromBody] CreateTodoRequest? req, [FromServices] ITodoStore store) =>
    {
        if (req is null)
            return Results.BadRequest(new { error = "Request body is required." });

        if (string.IsNullOrWhiteSpace(req.Title))
            return Results.BadRequest(new { error = "Title is required." });

        if (req.Title.Length > 200)
            return Results.BadRequest(new { error = "Title must be 200 characters or fewer." });

        var item = store.Add(req.Title.Trim());
        return Results.Created($"/api/todos/{item.Id}", item);
    })
    .Accepts<CreateTodoRequest>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("CreateTodo");

app.MapDelete("/todos/{id:int}",
    ([FromRoute] int id, [FromServices] ITodoStore store) =>
    {
        var removed = store.Delete(id);
        return removed ? Results.NoContent() : Results.NotFound();
    })
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("DeleteTodo");

app.Run();