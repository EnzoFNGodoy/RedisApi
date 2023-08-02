using Microsoft.EntityFrameworkCore;
using RedisApp.WebApi.Domain.Entities;

namespace RedisApp.WebApi.Infrastructure.Data;

public sealed class ToDoListContext : DbContext
{
    public ToDoListContext(DbContextOptions<ToDoListContext> options)
        : base(options)
    { }

    public DbSet<ToDo> ToDos => Set<ToDo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDo>()
            .HasKey(x => x.Id);
    }
}