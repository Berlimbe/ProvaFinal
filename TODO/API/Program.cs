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

app.MapPost("/api/tarefas", async (Tarefa tarefa, AppDataContext context) => //qualquer coisa tira o Id
{
    if(string.IsNullOrWhiteSpace(tarefa.Titulo)){ //não tenho certeza se é maiusculo
        return Results.BadRequest("Título é obrigatório.");
    }
    if(tarefa.Status is null){
        return Results.BadRequest("Status é obrigatório.");
    }

    context.Tarefas.Add(tarefa);
    await context.SaveChangesAsync();
    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
});
app.MapGet("/api/tarefas", async (AppDataContext context) =>
{
    return Results.Ok(await context.Tarefas.ToListAsync());
});

app.MapGet("/api/tarefas/{id}", async (int id, AppDataContext context) => 
{
    var tarefa = await context.Tarefas.FindAsync(id);
    return tarefa is null ? Results.NotFound("Tarefa com ID {id} não encontrada.") : Results.Ok(tarefa);
});


app.MapPut("/api/tarefas/{id}", async (int id, Tarefa tarefaAtualizada, AppDataContext context) => 
{
    var tarefaExist = await context.Tarefas.FindAsync(id);
    if(tarefaExist is null)
    {
        return Results.NotFound();
    }

    tarefaExist.Titulo = tarefaAtualizada.Titulo;
    await context.SaveChangesAsync();
    return Results.Ok(tarefaAtualizada.Titulo);

    tarefaExist.Status = tarefaAtualizada.Status;
    await context.SaveChangesAsync();
    return Results.Ok(tarefaAtualizada.Status);
});

/*
app.MapDelete("/api/tarefas/{id}", async (int id, AppDataContext context) =>
{
    var tarefa = await context.Tarefas.FindAsync(id);
    if(tarefa is null)
    {
        return Results.NotFound("Tarefa com ID {id} não encontrada para remoção.");
    }
    context.Tarefas.Remove(tarefa);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
*/

app.Run();
