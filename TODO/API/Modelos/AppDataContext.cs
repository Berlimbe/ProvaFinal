using Microsoft.EntityFrameworkCore;

namespace API.Modelos;

public class AppDataContext : DbContext
{
    public DbSet<Status> Status { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Status>().HasData(
            new Status() { Id = 1, Nome = "Pendente" },
            new Status() { Id = 2, Nome = "Em Desenvolvimento" },
            new Status() { Id = 3, Nome = "Concluída" }
        );
    }

}