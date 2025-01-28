
using Backend.Models;

namespace Backend.DTOs
{
public class ToDoListDto
{
        public string Message { get; set; } = string.Empty;

        public int UserID { get; set; }

        public Priority Priority { get; set; }
        
        public Category Category { get; set; }

}
}
