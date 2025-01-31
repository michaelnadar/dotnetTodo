

using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Backend.Services.TodoService
{
    public class TodoService:ITodoService{

        private readonly DataContext context;
         private readonly IHttpContextAccessor httpContextAccessor;

        public TodoService(DataContext _context,IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task<List<ToDoList>> CreateTodo(ToDoListDto request)
        {
            request.UserID = int.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
              var user = await context.Users.FindAsync(request.UserID);
                if (user == null)
                {
                    return null;
                }
                
                var newToDo = new ToDoList
                {
                    Message = request.Message,
                    UserID = request.UserID,
                    Priority = request.Priority,
                    Category = request.Category
                };
             try
            {
             context.ToDoLists.Add(newToDo);
            await context.SaveChangesAsync();
            return await context.ToDoLists
            .Where(t=>t.UserID == request.UserID)
            .ToListAsync();
                
            }
            catch (System.Exception)
            {
               return null;
            }
        }

         public async Task<List<ToDoList>?> GetToDoListsByUserId(){
            int userId = int.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
         var userExists = await context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return null;
        }

        var toDoLists = await context.ToDoLists
            .Where(t => t.UserID == userId)
            .ToListAsync();

        if (toDoLists == null || toDoLists.Count == 0)
        {
            return null;
        }

        return toDoLists;

         }


          public async Task<List<ToDoList>?> update(ToDoListDto request,int id){
             var userId = int.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
         var existingToDo = context.ToDoLists.Find(id);
            if(existingToDo is null){
                return null;
            }
            if(existingToDo.UserID != userId){
                     return null;
            }
            existingToDo.Message = request.Message;
            existingToDo.Priority = request.Priority;
            existingToDo.Category = request.Category;
            try
            {
            await context.SaveChangesAsync();
            return await context.ToDoLists
            .Where(t => t.UserID == userId)
            .ToListAsync();
                
            }
            catch (System.Exception)
            {
               return null;
            }
    }



     public  async Task<List<ToDoList>?> deleteSingle(int id){
         var userId = int.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
         Console.WriteLine($"Retrieved UserId: {userId}");
        var toDoItem = await context.ToDoLists
        .Where(t => t.Id == id && t.UserID == userId)
        .FirstOrDefaultAsync();

        if (toDoItem == null)
        {
            return null;
        }
            try
            {
                
                    context.ToDoLists.Remove(toDoItem);
                        await context.SaveChangesAsync();
                    return await context.ToDoLists
                    .Where(t => t.UserID == userId)
                    .ToListAsync();
            }
            catch (System.Exception)
            {
                
                     return null;
            }
    }

     public  async Task<string> deleteAllByUser(int userId){
         var currentUser = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(int.Parse(currentUser) != userId){
            return "You dont have access";
        }
      var userToDoItems = await context.ToDoLists
        .Where(t => t.UserID == userId)
        .ToListAsync();

        if (!userToDoItems.Any())
        {
            return "No ToDo list items found to delete for the specified user.";
        }

        context.ToDoLists.RemoveRange(userToDoItems);
        await context.SaveChangesAsync();

        return $"All ToDo list items for user {userId} have been deleted successfully.";
        }

    }
}