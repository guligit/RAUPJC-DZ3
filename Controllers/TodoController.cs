using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        // Inject user manager into constructor
        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUser();

            var todoItems = _repository.GetActive(Guid.Parse(currentUser.Id));

            return View(todoItems);
        }        public async Task<IActionResult> GetCompleted()
        {
            var currentUser = await GetCurrentUser();

            var todoItems = _repository.GetCompleted(Guid.Parse(currentUser.Id));

            return View(todoItems);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public IActionResult CreateTodoItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(AddTodoViewModel todo)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUser();
                _repository.Add(todo.GetTodoItem(Guid.Parse(user.Id)));
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            var currentUser = await GetCurrentUser();
            _repository.MarkAsCompleted(todoId, Guid.Parse(currentUser.Id));
            return RedirectToAction("Index");
        }
    }
}