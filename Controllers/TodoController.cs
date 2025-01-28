
using Backend.DTOs;
using Backend.Models;
using Backend.Services.TodoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController:ControllerBase{

        private readonly ITodoService todoService;
        public TodoController(ITodoService _todoService)
        {
            todoService = _todoService;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoList>> post(ToDoListDto request)
        {
            Console.WriteLine(request);
            var hello = await todoService.CreateTodo(request);
            return Ok(hello);

        }


    [HttpGet("todolists")]
    public async Task<ActionResult<List<ToDoList>>> GetToDoListsByUserId()
    {
       var todo = await todoService.GetToDoListsByUserId();
       if(todo is null){
        return NotFound("Not Found");
       }
       return Ok(todo);
    }

     [HttpPut("{id}")]
        public async  Task<ActionResult<ToDoList>> update(ToDoListDto request,int id)
        {   
          var hello = await todoService.update(request,id);
           if (hello is null ){
            return NotFound("not found");
           }
           return Ok(hello);

        }


          [HttpDelete("delete/{id}")]
        public async  Task<ActionResult<ToDoList>> deleteSingle(int id)
        {   
            var hello =await todoService.deleteSingle(id);

            if(hello is null)
                return NotFound("not found");

            return Ok(hello);

        }

          [HttpDelete("user/{userId}")]
        public async  Task<ActionResult<string>> deleteAllByUser(int userId)
        {   
            var hello =await todoService.deleteAllByUser(userId);

            if(hello is null)
                return NotFound("not found");

            return Ok(hello);

        }
    
    }
}