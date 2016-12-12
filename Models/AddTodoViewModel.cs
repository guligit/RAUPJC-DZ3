using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }

        public TodoItem GetTodoItem(Guid userId)
        {
            return new TodoItem(Text, userId);
        }
    }
}
