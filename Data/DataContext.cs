global using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


namespace Backend.Data;
using Backend.Models;

public class DataContext: DbContext
{

public DataContext(DbContextOptions<DataContext> options): base(options)
{
    
}


protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.UseMySql("Server=localhost;Database=test;User=root;Password=micheal@1;",
                new MySqlServerVersion(new Version(10, 5, 0)) );
}

         public DbSet<User> Users { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ToDoList>()
        .HasOne(t => t.User)
        .WithMany(u => u.ToDoLists)
        .HasForeignKey(t => t.UserID)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ToDoList>()
        .Property(t => t.Priority)
        .HasConversion<string>();  // Store enum as string

    modelBuilder.Entity<ToDoList>()
        .Property(t => t.Category)
        .HasConversion<string>();  // Store enum as string

         base.OnModelCreating(modelBuilder);
}


}