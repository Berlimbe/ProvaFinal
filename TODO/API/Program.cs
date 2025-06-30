using API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

app.MapGet("/", (AppDataContext context) =>
{
    var status = context.Status.ToList();
    return Results.Ok(status);
});


app.Run();
