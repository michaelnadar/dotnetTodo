

using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.TodoService
{
    public interface ITodoService{

        Task<List<ToDoList>> CreateTodo(ToDoListDto request);

        Task<List<ToDoList>?> GetToDoListsByUserId();

        Task<List<ToDoList>?> update(ToDoListDto request,int id);

        Task<List<ToDoList>?> deleteSingle(int id);

        Task<string?> deleteAllByUser(int id);

    }
}