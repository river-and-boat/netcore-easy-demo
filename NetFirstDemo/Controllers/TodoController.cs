using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFirstDemo.Model;
using NetFirstDemo.Service;

namespace NetFirstDemo.Controllers
{
    [ApiController]
    [Route("api")]
    public class TodoController : ControllerBase
    {
        private readonly TodoListService service;

        public TodoController(TodoListService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("items/{id}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            return Ok(await service.GetTodoItemAsync(id));
        }

        [HttpPost]
        [Route("items")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<TodoItem>> AddTodoItem(TodoItem todoItem)
        {
            TodoItem result = await service.AddTodoItemAsync(todoItem);
            Console.WriteLine(result);
            return CreatedAtAction(nameof(GetTodoItem), new { id = result.Id }, result);
        }

        [HttpPut]
        [Route("/items")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult> PutTodoItem(TodoItem todoItem)
        {
            try
            {
                await service.UpdateTodoItemAsync(todoItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("some exception occured when update todo item: {0}", ex);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/items")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult> DeleteTodoItem(long id)
        {
            try
            {
                await service.DeleteTodoItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("some exception occured when update todo item: {0}", ex);
                return BadRequest();
            }
        }
    }
}
