using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetFirstDemo.Data;
using NetFirstDemo.Model;

namespace NetFirstDemo.Service
{
    public class TodoListService
    {
        private readonly TodoContext context;

        public TodoListService(TodoContext context)
        {
            this.context = context;
        }

        public async Task<TodoItem> GetTodoItemAsync(long id)
        {
            return await context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem> AddTodoItemAsync(TodoItem todoItem)
        {
            await context.AddAsync(todoItem);
            await context.SaveChangesAsync();
            return todoItem;
        }

        public async Task UpdateTodoItemAsync(TodoItem newTodoItem)
        {
            TodoItem oldTodoItem = await context.TodoItems.FindAsync(newTodoItem.Id);
            if (oldTodoItem == null)
            {
                throw new Exception("can not find given todo item");
            }
            UpdateTodoItem(oldTodoItem, newTodoItem);
            context.Entry(oldTodoItem).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private void UpdateTodoItem(TodoItem oldTodoItem, TodoItem newTodoItem) {
            oldTodoItem.Id = newTodoItem.Id;
            oldTodoItem.Name = newTodoItem.Name;
            oldTodoItem.IsCompleted = newTodoItem.IsCompleted;
        }

        public async Task DeleteTodoItemAsync(long id)
        {
            TodoItem todoItem = await context.TodoItems.FindAsync(id);
            if(todoItem == null)
            {
                throw new Exception("can not find given todo item");
            }
            context.TodoItems.Remove(todoItem);
            await context.SaveChangesAsync();
        }
    }
}
