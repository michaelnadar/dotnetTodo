
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
        public async Task<ActionResult<ToDoList>> Create(ToDoListDto request)
        {
            try{
            var data = await todoService.CreateTodo(request); 
            return Ok(data);
            }catch(Exception ex){
                    return StatusCode(StatusCodes.Status500InternalServerError,"500 Server Err");
            }

        }


    [HttpGet("todolists")]
    public async Task<ActionResult<List<ToDoList>>> GetToDoListsByUserId()
    {
       try
       {  
       var todo = await todoService.GetToDoListsByUserId();
       return Ok(todo);
       }
       catch (Exception ex)
       {
            return StatusCode(StatusCodes.Status500InternalServerError,"500 Server Err");
       }
    }

     [HttpPut("{id}")]
        public async  Task<ActionResult<ToDoList>> Update(ToDoListDto request,int id)
        {   
            try
            {
                var todo = await todoService.update(request,id);
                if (todo is null ){
                    return NotFound("not found");
                }
                return Ok(todo);
            }
            catch (Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError,"500 Server Err");
            }

        }


          [HttpDelete("delete/{id}")]
        public async  Task<ActionResult<ToDoList>> DeleteSingle(int id)
        {   
            try
            {
            var delete =await todoService.deleteSingle(id);
            if(delete is null)
                return NotFound("not found");
            return Ok(delete);
                
            }
            catch (Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError,"500 Server Err");
            }

        }

          [HttpDelete("user/{userId}")]
        public async  Task<ActionResult<string>> DeleteAllByUser(int userId)
        {   
            var hello =await todoService.deleteAllByUser(userId);

            if(hello is null)
                return NotFound("not found");

            return Ok(hello);

        }
    
    }
}