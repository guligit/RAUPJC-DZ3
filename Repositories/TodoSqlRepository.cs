using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Interfaces;
using ToDoList.Models;
using ToDoList.Exceptions;

namespace ToDoList.Repositories
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public void Add(TodoItem todoItem)
        {
            if (_context.TodoItems.Find(todoItem.Id) != null)
            {
                throw new DuplicateTodoItemException();
            }

            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var todoItem = _context.TodoItems.Find(todoId);

            if (todoItem.UserId != userId)
            {
                throw new TodoAccessDeniedException();
            }

            return todoItem;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return GetFiltered(i => !i.IsCompleted, userId);
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return GetFiltered(i => i.UserId == userId, userId);
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return GetFiltered(i => i.IsCompleted, userId);
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(i => filterFunction(i) && i.UserId == userId)
                                     .OrderByDescending(i => i.DateCreated)
                                     .ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var todoItem = Get(todoId, userId);

            if (todoItem == null)
            {
                return false;
            }

            todoItem.MarkAsCompleted();
            Update(todoItem, userId);

            return true;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todoItem = Get(todoId, userId);

            if (todoItem == null)
            {
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();

            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            var newTodoItem = Get(todoItem.Id, userId);

            if (newTodoItem == null)
            {
                Add(todoItem);
            }
            else
            {
                _context.Entry(todoItem).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
