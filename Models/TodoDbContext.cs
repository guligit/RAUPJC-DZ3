using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ToDoList.Models
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(string connectionString) : base(connectionString)
        {
        }

        public IDbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItem>().Property(i => i.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCompleted).IsOptional();
        }
    }
}
